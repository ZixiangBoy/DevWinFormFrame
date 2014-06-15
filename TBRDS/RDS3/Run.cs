using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Top.Api.Domain;
using Top.Api.Response;
using Top.Api.Util;
using Ultra.Log;
using Ultra.Web.Core.Common;

namespace RDS3
{
    public class Run
    {
        private string _con = string.Empty;

        private string Proc = "P_ERP_AutoTakeSync";
        private string RefundProc = "P_ERP_AutoTakeRefundSync";
        private string ProcShop = "P_ERP_AutoTakeSyncByNick";//按店铺提取需要同步的订单
        private string RefundProcShop = "P_ERP_AutoTakeRefundSyncByNick";//按店铺提取需要同步的退款单
        private string ProcShopRng = "P_ERP_TakeSyncRangeByNick";//获取需要同步的店铺的时间范围
        private string ProcShopRngSync = "P_ERP_AutoTakeSyncByNick";//获取需要同步的店铺的时间范围内的订单
        private string ProcShopRetry = "P_ERP_RetryTakeSyncByNick";//重试

        private readonly int CONTINUE_CODE = -200;//
        private readonly int CONTINUE_MIN = 10;

        public bool Trace = false;

        private ApplicationLog _applog = null;

        private int _sec = 180;

        //文件保存的路径
        private string _filepath;

        private object objLock = new object();

        private Queue<T_ERP_SyncLog> QueFail = new Queue<T_ERP_SyncLog>(30);

        private static string CurDir
        {
            get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory); }
        }

        public Run(string conn, ApplicationLog applog, int sec = 180)
        {
            _con = conn;
            _sec = sec;
            _applog = applog;
        }
        public Run(string conn, ApplicationLog applog,string filepath, int sec = 180)
        {
            _con = conn;
            _sec = sec;
            _filepath = filepath;
            _applog = applog;
        }

        string[] Exclude = new string[] { 
            "Id","PId",
            //"Guid",
            "OrderList",
            "PromotList",
            "ServiceOrderList"
        };

        protected void WriteData(List<Jdp> jdps)
        {
            try
            {
                //List<TradeFullinfoGetResponse> trdf = jdps.Select(k => TopUtils.ParseResponse<TradeFullinfoGetResponse>(k.jdp_response))
                //    .Where(j => !string.IsNullOrEmpty(j.Trade.PayTime)).ToList();
                List<TradeFullinfoGetResponse> trdf = jdps.Select(k => TopUtils.ParseResponse<TradeFullinfoGetResponse>(k.jdp_response)).ToList();
                if (null == trdf || trdf.Count < 1) return;
                var trdpayed = trdf.Select(k => k.Trade).ToList();
                if (null == trdpayed || trdpayed.Count < 1) return;
                var trdpay = Caster.CastTrades(trdpayed);
                if (null == trdpay || trdpay.Count < 1) return;
                using (var blk = new SqlBulkCopy(_con))
                {
                    blk.DestinationTableName = "T_ERP_TaoBaoATS_Trade";
                    blk.WriteToServerAdv<T_ERP_TaoBao_Trade>(trdpay, Exclude);

                    blk.DestinationTableName = "T_ERP_TaoBaoATS_Order";
                    var ords = trdpay.SelectMany(k => k.OrderList).ToList();
                    blk.WriteToServerAdv<T_ERP_TaoBao_Order>(ords, Exclude);

                    blk.DestinationTableName = "T_ERP_TaoBaoATS_PromotionDetail";
                    var kpromt = trdpay.Where(j => j.PromotList != null).SelectMany(k => k.PromotList);
                    if (null != kpromt)
                    {
                        try
                        {
                            var promots = kpromt.ToList();
                            if (null != promots && promots.Count > 0)
                                blk.WriteToServerAdv<T_ERP_TaoBao_PromotionDetail>(promots, Exclude);
                        }
                        catch (Exception ex) { throw; }
                    }

                    blk.DestinationTableName = "T_ERP_TaobaoATS_ServiceOrder";
                    var ksvcs = trdpay.Where(j => j.ServiceOrderList != null).SelectMany(k => k.ServiceOrderList);
                    if (null != ksvcs)
                    {
                        try
                        {
                            var svcs = ksvcs.ToList();
                            if (null != svcs && svcs.Count > 0)
                                blk.WriteToServerAdv<T_ERP_Taobao_ServiceOrder>(svcs, Exclude);
                        }
                        catch (Exception ex) { throw; }
                    }
                }

            }
            catch (Exception ex)
            {
                _applog.DebugException(ex);
#if DEBUG
                throw;
#else
#endif
            }

        }


        protected void WriteDataEx(List<Jdp> jdps, ShopData shp, DateTime begin, DateTime end, string remark = "同步")
        {
            try
            {
                List<TradeFullinfoGetResponse> trdf = jdps.Select(k => TopUtils.ParseResponse<TradeFullinfoGetResponse>(k.jdp_response))
                    .Where(j => !string.IsNullOrEmpty(j.Trade.PayTime)).ToList();
                if (null == trdf || trdf.Count < 1) return;

                using (var blk = new SqlBulkCopy(_con))
                {
                    blk.DestinationTableName = "T_ERP_SyncLog";
                    blk.WriteToServerAdv<T_ERP_SyncLog>(new List<T_ERP_SyncLog> { new T_ERP_SyncLog{
                        SellerNick=shp.SellerNick,MinDateTime=begin,MaxDateTime=end,Remark=remark,JdpCounts=jdps.Count,JdpResCounts=trdf.Count
                    }}, Exclude);
                }

                var trdpayed = trdf.Select(k => k.Trade).ToList();
                if (null == trdpayed || trdpayed.Count < 1) return;
                var trdpay = Caster.CastTrades(trdpayed);
                if (null == trdpay || trdpay.Count < 1) return;
                using (var blk = new SqlBulkCopy(_con))
                {
                    blk.DestinationTableName = "T_ERP_TaoBaoATS_Trade";
                    blk.WriteToServerAdv<T_ERP_TaoBao_Trade>(trdpay, Exclude);

                    blk.DestinationTableName = "T_ERP_TaoBaoATS_Order";
                    var ords = trdpay.SelectMany(k => k.OrderList).ToList();
                    blk.WriteToServerAdv<T_ERP_TaoBao_Order>(ords, Exclude);

                    blk.DestinationTableName = "T_ERP_TaoBaoATS_PromotionDetail";
                    var kpromt = trdpay.Where(j => j.PromotList != null).SelectMany(k => k.PromotList);
                    if (null != kpromt)
                    {
                        try
                        {
                            var promots = kpromt.ToList();
                            if (null != promots && promots.Count > 0)
                                blk.WriteToServerAdv<T_ERP_TaoBao_PromotionDetail>(promots, Exclude);
                        }
                        catch (Exception ex) { _applog.DebugException(new Exception(string.Format("SHOP:{0} Promot Error:{1}", shp.SellerNick, ex.Message))); }
                    }

                    blk.DestinationTableName = "T_ERP_TaobaoATS_ServiceOrder";
                    var ksvcs = trdpay.Where(j => j.ServiceOrderList != null).SelectMany(k => k.ServiceOrderList);
                    if (null != ksvcs)
                    {
                        try
                        {
                            var svcs = ksvcs.ToList();
                            if (null != svcs && svcs.Count > 0)
                                blk.WriteToServerAdv<T_ERP_Taobao_ServiceOrder>(svcs, Exclude);
                        }
                        catch (Exception ex) { _applog.DebugException(new Exception(string.Format("SHOP:{0} SvcOrder Error:{1}", shp.SellerNick, ex.Message))); }
                    }
                }
                //登记同步时间
                if (remark != "重试")
                    _applog.DebugException(new Exception(string.Format("SHOP:{0} Begin:{1} End:{2} Count:{3}", shp.SellerNick, begin.ToDefaultStr(), end.ToDefaultStr(), jdps.Count)));
                else
                    _applog.DebugException(new Exception(string.Format("RETRY\tSHOP:{1}\tCOUNT:{0}\tBegin:{2}\tEnd:{3}\tFor Retry Sync.", jdps.Count, shp.SellerNick,
                    begin.ToDefaultStr(), end.ToDefaultStr())));
            }
            catch (Exception ex)
            {
                //_applog.DebugException(new Exception(string.Format("Shop:{0} Begin:{2} End:{3} Ex:{1}", shp.SellerNick, ex.Message,
                //    begin.ToDefaultStr(), end.ToDefaultStr())));
                //lock (objLock)//加入失败队列重新操作
                //{
                //    QueFail.Enqueue(new T_ERP_SyncLog
                //    {
                //        SellerNick = shp.SellerNick,
                //        MinDateTime = begin,
                //        MaxDateTime = end
                //    });
                //}
#if DEBUG
                //throw;
#else
#endif
            }

        }

        void RetryFail()
        {
            List<T_ERP_SyncLog> failList = null;
            lock (objLock)
            {
                failList = QueFail.ToList();
                QueFail.Clear();
            }
            if (null == failList || failList.Count < 1)
            {
                _applog.DebugException(new Exception(string.Format("No Task For Retry")));
                return;
            }
            _applog.DebugException(new Exception(string.Format("{0} Task For Retry", failList.Count)));
            foreach (var item in failList)
            {
                if (null == item) continue;
                SyncRetry(item);
            }
        }

        protected void WriteRefundData(List<Refund> refs)
        {
            try
            {
                var trdpay = Caster.CastRefund(refs);
                using (var blk = new SqlBulkCopy(_con))
                {
                    blk.DestinationTableName = "T_ERP_TaoBao_Refund";
                    blk.WriteToServerAdv<T_ERP_TaoBao_Refund>(trdpay,new string[]{"Id","CreateTime"});
                }

            }
            catch (Exception ex)
            {
                _applog.DebugException(ex);
#if DEBUG
                throw;
#else
#endif
            }
        }

        protected void Sync()
        {
            try
            {
                var dt = SqlHelper.ExecuteDataTable(_con, System.Data.CommandType.StoredProcedure, Proc);
                if (dt == null || dt.Rows.Count < 1) return;
                var jds = ObjectHelper.Create<Jdp>(dt);
                if (null == jds || jds.Count < 1) return;
                _applog.DebugException(new Exception(string.Format("COUNT:{0} For Sync.", jds.Count)));
                WriteData(jds);
            }
            catch (Exception ex)
            {
                _applog.DebugException(ex);
#if DEBUG
                throw;
#else
#endif
            }
        }

        protected int SyncRetry(T_ERP_SyncLog sd)
        {
            try
            {
                var prmMin = new SqlParameter("@minjdp", sd.MinDateTime);
                var prmMax = new SqlParameter("@maxjdp", sd.MaxDateTime);
                var prmNick = new SqlParameter("@Nick", sd.SellerNick);

                var dt = SqlHelper.ExecuteDataTable(_con, System.Data.CommandType.StoredProcedure, ProcShopRetry, prmNick, prmMin, prmMax);
                if (dt == null || dt.Rows.Count < 1) return 0;
                var jds = ObjectHelper.Create<Jdp>(dt);
                if (null == jds || jds.Count < 1) return 0;
                //_applog.DebugException(new Exception(string.Format("RETRY\tSHOP:{1}\tCOUNT:{0}\tBegin:{1}\tEnd:{2}\tFor Retry Sync.", jds.Count, sd.SellerNick,
                //    sd.MinDateTime.ToDefaultStr(),sd.MaxDateTime.ToDefaultStr())));

                DateTime begin = (DateTime)(prmMin.Value);
                DateTime end = (DateTime)(prmMax.Value);

                var shop = GetShops().Where(p => p.SellerNick.Equals(sd.SellerNick)).FirstOrDefault();

                Guid shpguid = Guid.NewGuid();
                List<TradeFullinfoGetResponse> trdf = jds.Select(k => TopUtils.ParseResponse<TradeFullinfoGetResponse>(k.jdp_response))
                        .ToList();
                var trds = trdf.Select(k => k.Trade).ToList();
                var shpTrd = new T_ERP_ShopTrade()
                {
                    Begin = begin,
                    End = end,
                    Guid = shpguid,
                    SellerNick = shop.SellerNick,
                    JdpCount = trds.Count(),
                    TrdPayed = trds.Where(p => !string.IsNullOrEmpty(p.PayTime)).Count(),
                    TrdUnPay = trds.Where(p => string.IsNullOrEmpty(p.PayTime)).Count(),
                    TrdStep = trds.Where(p => !string.IsNullOrEmpty(p.StepTradeStatus)).Count()
                };

                var fileName = GetTrdFileID(shop, begin, end, shpTrd);
                if (string.IsNullOrEmpty(fileName)) return 0;
                WriteFileTradeData(trds, fileName);
                //WriteDataEx(jds, new ShopData { SellerNick = sd.SellerNick }, sd.MinDateTime, sd.MaxDateTime, "重试");
                return jds.Count;
            }
            catch (Exception ex)
            {
                _applog.DebugException(ex);
#if DEBUG
                //throw;
#else
#endif
                return 100;
            }
        }

        protected int SyncShop2(object shp)
        {
            var shop = shp as ShopData;

            var prmMin = new SqlParameter("@minjdp", DateTime.Now); prmMin.Direction = System.Data.ParameterDirection.Output;
            var prmMax = new SqlParameter("@maxjdp", DateTime.Now); prmMax.Direction = System.Data.ParameterDirection.Output;
            DateTime begin = (DateTime)(prmMin.Value);
            DateTime end = (DateTime)(prmMax.Value);
            try
            {
                var prmNick = new SqlParameter("@Nick", shop.SellerNick); 
                SqlHelper.ExecuteNonQuery(_con, System.Data.CommandType.StoredProcedure, ProcShopRng, prmNick, prmMin, prmMax);
                prmMin.Direction = System.Data.ParameterDirection.Input;
                prmMax.Direction = System.Data.ParameterDirection.Input;
                var dt = SqlHelper.ExecuteDataTable(_con, System.Data.CommandType.StoredProcedure, ProcShopRngSync, prmNick, prmMin, prmMax);
                begin = (DateTime)(prmMin.Value);
                end = (DateTime)(prmMax.Value);
                if (dt == null || dt.Rows.Count < 1)
                {
                    //判断时间上是否远远落后于当前服务器时间，如果是(落后超过十分钟)则马上开始下一轮同步服务,以避免订单同步滞后
                    if (end.DateDiff(EnDatePart.MINUTE,begin)>=1 && TimeSync.Default.CurrentSyncTime.DateDiff(EnDatePart.MINUTE, end) >= CONTINUE_MIN)
                    {
                        _applog.DebugException(new Exception(string.Format("SHOP:{0}\t:Continue Next For Sync.SvrTime:{1}\tBegin:{2}\tEnd:{3}", shop.SellerNick,
                            TimeSync.Default.CurrentSyncTime.ToDefaultStr(), begin.ToDefaultStr(), end.ToDefaultStr())));
                        return CONTINUE_CODE;
                    }
                    else
                        return 0;
                }
                var jds = ObjectHelper.Create<Jdp>(dt);
                if (null == jds || jds.Count < 1)
                {
                    //判断时间上是否远远落后于当前服务器时间，如果是(落后超过十分钟)则马上开始下一轮同步服务,以避免订单同步滞后
                    if (end.DateDiff(EnDatePart.MINUTE, begin) >= 1 && TimeSync.Default.CurrentSyncTime.DateDiff(EnDatePart.MINUTE, end) >= CONTINUE_MIN)
                    {
                        _applog.DebugException(new Exception(string.Format("SHOP:{0}\t:Continue Next For Sync.SvrTime:{1}\tBegin:{2}\tEnd:{3}", shop.SellerNick,
                            TimeSync.Default.CurrentSyncTime.ToDefaultStr(),begin.ToDefaultStr(),end.ToDefaultStr())));
                        return CONTINUE_CODE;
                    }
                    else
                        return 0;
                }
                //_applog.DebugException(new Exception(string.Format("SHOP:{1}\tCOUNT:{0}\tFor Sync.", jds.Count, shop.SellerNick)));
                //WriteData(jds);


                List<Trade> trds;
                T_ERP_ShopTrade shpTrd = GetShopTrade(shop, begin, end, jds, out trds);
                var fileName = GetTrdFileID(shop, begin, end, shpTrd);
                if (string.IsNullOrEmpty(fileName) || trds.Count <= 0) return 0;
                WriteFileTradeData(trds, fileName);
                //WriteDataEx(jds, shop, begin, end);
                return jds.Count;
            }
            catch (Exception ex)
            {
                _applog.DebugException(new Exception(string.Format("SyncError\tSHOP:{0}\tBegin:{1}\tEnd:{2} ERR:{3}", shop.SellerNick, begin.ToDefaultStr(), end.ToDefaultStr()
                    , ex.Message)));
#if DEBUG
                //throw;
#else
#endif
                return 100;
            }
        }

        private T_ERP_ShopTrade GetShopTrade(ShopData shop, DateTime begin, DateTime end, List<Jdp> jds, out List<Trade> trds)
        {
            Guid shpguid = Guid.NewGuid();
            List<TradeFullinfoGetResponse> trdf = jds.Select(k => TopUtils.ParseResponse<TradeFullinfoGetResponse>(k.jdp_response))
                    .ToList();
            trds = trdf.Select(k => k.Trade).ToList();
            var payed = trds.Where(p => !string.IsNullOrEmpty(p.PayTime));
            var unpay = trds.Where(p => string.IsNullOrEmpty(p.PayTime));
            var step = trds.Where(p => !string.IsNullOrEmpty(p.StepTradeStatus));
            return new T_ERP_ShopTrade()
            {
                Begin = begin,
                End = end,
                Guid = shpguid,
                SellerNick = shop.SellerNick,
                JdpCount = trds.Count(),
                TrdPayed = payed==null ? 0 : payed.Count(),
                TrdUnPay = unpay==null ? 0 : unpay.Count(),
                TrdStep = step==null ? 0 : step.Count()
            };
        }

        private string GetTrdFileID(ShopData shop, DateTime begin, DateTime end, T_ERP_ShopTrade shpTrd)
        {
            try
            {
                using (var blk = new SqlBulkCopy(_con))
                {
                    blk.DestinationTableName = shop.TradeTableName;
                    blk.WriteToServer<T_ERP_ShopTrade>(shpTrd);
                }
                var fileId = SqlHelper.ExecuteScalar(_con, System.Data.CommandType.Text,
                    string.Format("select id from {0} where guid='{1}'", shop.TradeTableName, shpTrd.Guid));
                var fileName = _filepath + "trade\\" + shop.SellerNick + "\\" + fileId.ToString() + ".json";
                if (!File.Exists(_filepath + "test.txt"))
                {
                    _applog.DebugException(new Exception("路径不合法或者测试文件不存在!"));
                    return string.Empty;
                }
                return fileName;
            }
            catch (Exception ex)
            {
                _applog.DebugException(ex);
#if DEBUG
                throw;
#else
#endif
            }
        }

        protected int SyncShop(object shp)
        {
            var shop = shp as ShopData;
            try
            {
                var dt = SqlHelper.ExecuteDataTable(_con, System.Data.CommandType.StoredProcedure, ProcShop, new SqlParameter("@Nick", shop.SellerNick));
                if (dt == null || dt.Rows.Count < 1) return 0;
                var jds = ObjectHelper.Create<Jdp>(dt);
                if (null == jds || jds.Count < 1) return 0;
                _applog.DebugException(new Exception(string.Format("SHOP:{1}\tCOUNT:{0}\tFor Sync.", jds.Count, shop.SellerNick)));
                WriteData(jds);
                return dt.Rows.Count;
            }
            catch (Exception ex)
            {
                _applog.DebugException(ex);
#if DEBUG
                //throw;
#else
#endif
                return 100;
            }
        }

        protected void SyncRefund()
        {
            try
            {
                var dt = SqlHelper.ExecuteDataTable(_con, System.Data.CommandType.StoredProcedure, RefundProc);
                if (dt == null || dt.Rows.Count < 1) return;
                var jds = ObjectHelper.Create<Jdp>(dt);
                if (null == jds || jds.Count < 1) return;
                _applog.DebugException(new Exception(string.Format("COUNT:{0} For Refund Sync.", jds.Count)));
                //WriteRefundData(jds);
            }
            catch (Exception ex)
            {
                _applog.DebugException(ex);
#if DEBUG
                throw;
#else
#endif
            }
        }

        protected int SyncRefundShop(object shp)
        {
            var shop = shp as ShopData;
            var prmMin = new SqlParameter("@minjdp", DateTime.Now); prmMin.Direction = System.Data.ParameterDirection.Output;
            var prmMax = new SqlParameter("@maxjdp", DateTime.Now); prmMax.Direction = System.Data.ParameterDirection.Output;
            DateTime begin = (DateTime)(prmMin.Value);
            DateTime end = (DateTime)(prmMax.Value);
            try
            {
                var dt = SqlHelper.ExecuteDataTable(_con, System.Data.CommandType.StoredProcedure, RefundProcShop, new SqlParameter("@Nick", shop.SellerNick),prmMin,prmMax);
                begin = (DateTime)(prmMin.Value);
                end = (DateTime)(prmMax.Value);
                if (dt == null || dt.Rows.Count < 1) return 0;
                var jds = ObjectHelper.Create<Jdp>(dt);
                if (null == jds || jds.Count < 1) return 0;
                _applog.DebugException(new Exception(string.Format("SHOP:{1}\tCOUNT:{0}\tFor Refund Sync.", jds.Count, shop.SellerNick)));

                List<Refund> refs;
                T_ERP_ShopRefund shpRef= GetShopRefund(shop, begin, end, jds, out refs);
                var fileName = GetRefFileID(shop, begin, end, shpRef);
                if (string.IsNullOrEmpty(fileName) || refs.Count<=0) return 0;
                WriteFileRefundData(refs, fileName);
                WriteRefundData(refs);
                return dt.Rows.Count;
            }
            catch (Exception ex)
            {
                _applog.DebugException(ex);
#if DEBUG
                //throw;
#else
#endif
                return 0;
            }
        }

        private T_ERP_ShopRefund GetShopRefund(ShopData shop, DateTime begin, DateTime end, List<Jdp> jds, out List<Refund> refs)
        {
            List<RefundGetResponse> trdf = jds.Select(k => TopUtils.ParseResponse<RefundGetResponse>(k.jdp_response))
                                .ToList();
            refs = trdf.Select(k => k.Refund).ToList();
            Guid shpguid = Guid.NewGuid();
            return new T_ERP_ShopRefund()
            {
                Begin = begin,
                End = end,
                Guid = shpguid,
                SellerNick = shop.SellerNick,
                JdpCount = refs.Count
            };
        }

        private string GetRefFileID(ShopData shop, DateTime begin, DateTime end,T_ERP_ShopRefund shpRef)
        {
            try
            {
                using (var blk = new SqlBulkCopy(_con))
                {
                    blk.DestinationTableName = shop.RefundTableName;
                    blk.WriteToServer<T_ERP_ShopRefund>(shpRef);
                }
                var fileId = SqlHelper.ExecuteScalar(_con, System.Data.CommandType.Text,
                    string.Format("select id from {0} where guid='{1}'", shop.RefundTableName, shpRef.Guid));
                var fileName = _filepath + "refund\\" + shop.SellerNick + "\\" + fileId.ToString() + ".json";
                if (!File.Exists(_filepath + "test.txt"))
                {
                    _applog.DebugException(new Exception("路径不合法或者测试文件不存在!"));
                    return string.Empty;
                }
                return fileName;
            }
            catch (Exception ex)
            {
                _applog.DebugException(ex);
#if DEBUG
                throw;
#else
#endif
            }
        }

        protected void WriteFileRefundData(List<Refund> refs,string fileName)
        {
            var RetryCount = 0;
            //序列化成Json
            var refundJson = ObjectHelper.SerializeJson(refs);
            Retry://重试
            try
            {
                if (!File.Exists(fileName))
                {
                    //FileStream fs = new FileStream(fileName, FileMode.CreateNew);
                    //StreamWriter sw = new StreamWriter(fs);
                    //sw.Write(refundJson);
                    //sw.Flush();
                    File.WriteAllText(fileName, refundJson, Encoding.UTF8);
                }
            }
            catch (Exception ex)
            {
                _applog.DebugException(ex);
                if (RetryCount > 3)
                {
                    var wrterr = new T_ERP_WriteFileErr()
                    {
                        TypeName = "退款",
                        FailFileName = fileName,
                        JsonText = refundJson
                    };
                    using (var blk = new SqlBulkCopy(_con))
                    {
                        blk.DestinationTableName = "T_ERP_WriteFileErr";
                        blk.WriteToServer<T_ERP_WriteFileErr>(wrterr);
                    }
                }
                else
                {
                    RetryCount++;
                    goto Retry;
                }
#if DEBUG
                throw;
#else
#endif
            }
        }

        protected void WriteFileTradeData(List<Trade> trds, string fileName)
        {
            var RetryCount = 0;
            //序列化成Json
            var tradeJson = ObjectHelper.SerializeJson(trds);
            Retry:
            try
            {
                if (!File.Exists(fileName))
                {
                    //FileStream fs = new FileStream(fileName, FileMode.CreateNew);
                    //StreamWriter sw = new StreamWriter(fs);
                    //sw.Write(tradeJson);
                    //sw.Flush();
                    File.WriteAllText(fileName, tradeJson, Encoding.UTF8);
                }
            }
            catch (Exception ex)
            {
                _applog.DebugException(ex);
                if (RetryCount > 3)
                {
                    var wrterr = new T_ERP_WriteFileErr()
                    {
                        TypeName = "订单",
                        FailFileName = fileName,
                        JsonText = tradeJson
                    };
                    using (var blk = new SqlBulkCopy(_con))
                    {
                        blk.DestinationTableName = "T_ERP_WriteFileErr";
                        blk.WriteToServer<T_ERP_WriteFileErr>(wrterr);
                    }
                }
                else
                {
                    RetryCount++;
                    goto Retry;
                }
#if DEBUG
                throw;
#else
#endif
            }
        }

        /// <summary>
        /// 开始数据同步
        /// </summary>
        public void StartSyncEx()
        {
            //开启服务器时间同步
            var dt = SqlHelper.ExecuteDataTable(_con, System.Data.CommandType.Text, "select getdate() CurrentTime");
            var svrTime = ObjectHelper.Create<ServerTime>(dt);
            TimeSync.Default.StartSync(svrTime[0].CurrentTime);
            //////////////////////////////////////////////////////////////
            var tRetry = new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(60000);
                    RetryFail();
                }
            });
            tRetry.IsBackground = true;
            tRetry.SetApartmentState(ApartmentState.STA);
            tRetry.Start();

            //获取需要同步订单的店铺数量
            Shops = GetShops();
            if (null == Shops || Shops.Count < 1) return;
            foreach (var shp in Shops)
            {
                //_applog.DebugException(new Exception(string.Format("Shop:{0} Sync Start.", shp.SellerNick)));
                SyncByShopEx(shp);
            }
        }

        public void SyncByShopEx(object shp)
        {
            var shop = shp as ShopData;
            var t = new Thread(() =>
            {
                int count = 0;
                while (true)
                {
                    count = SyncShop2(shop);
                    if (count != CONTINUE_CODE && count < 1)
                    {
                        if (Trace)
                        {
                            _applog.DebugException(new Exception(string.Format("Shop:{0}\t Sync Trade Wait For Next Call.", shop.SellerNick)));
                        }
                        Thread.Sleep(_sec);
                    }
                }
            });
            t.IsBackground = true;
            t.SetApartmentState(ApartmentState.STA);
            t.Start();

            var t2 = new Thread(() =>
            {
                int count = 0;
                while (true)
                {
                    count = SyncRefundShop(shop);
                    if (count < 1)
                    {
                        //if (Trace)
                        //{
                        //    _applog.DebugException(new Exception(string.Format("Shop:{0}\t Sync Refund Wait For Next Call.", shop.SellerNick)));
                        //}
                        Thread.Sleep(_sec);
                    }
                }
            });
            t2.IsBackground = true;
            t2.SetApartmentState(ApartmentState.STA);
            t2.Start();
        }

        /// <summary>
        /// 按店铺同步订单与退款
        /// </summary>
        /// <param name="shp"></param>
        public void SyncByShop(object shp)
        {
            var shop = shp as ShopData;
            if (null == shop) return;
            var adjsec = shop.SleepSecAdjust;
            var t = new Thread(() =>
            {
                int count = 0;
                while (true)
                {
                    count = SyncShop(shop);
                    if (count < 1)
                    {
                        if (Trace)
                        {
                            _applog.DebugException(new Exception(string.Format("Shop:{0}\t Sync Trade Wait For Next Call.", shop.SellerNick)));
                        }
                    }
                    Thread.Sleep(_sec);
                }
            });
            t.IsBackground = true;
            t.SetApartmentState(ApartmentState.STA);
            t.Start();

            var t2 = new Thread(() =>
            {
                int count = 0;
                while (true)
                {
                    count = SyncRefundShop(shop);
                    if (count < 1)
                    {
                        if (Trace)
                        {
                            _applog.DebugException(new Exception(string.Format("Shop:{0}\t Sync Refund Wait For Next Call.", shop.SellerNick)));
                        }
                        Thread.Sleep(_sec);
                    }
                }
            });
            t2.IsBackground = true;
            t2.SetApartmentState(ApartmentState.STA);
            t2.Start();
        }

        List<ShopData> Shops { get; set; }

        /// <summary>
        /// 获取店铺数据实体
        /// </summary>
        /// <returns></returns>
        List<ShopData> GetShops()
        {
            var dt = SqlHelper.ExecuteDataTable(_con, System.Data.CommandType.Text, "select distinct SellerNick,TradeTableName,RefundTableName from T_ERP_Sync_Shop nolock");
            return ObjectHelper.Create<ShopData>(dt);
        }
    }
}
