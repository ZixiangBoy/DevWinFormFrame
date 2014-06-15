using System;
using System.Collections.Generic;
using Top.Api.Response;
using Top.Api.Util;

namespace Top.Api.Request
{
    /// <summary>
    /// TOP API: alibaba.logistics.route.query
    /// </summary>
    public class AlibabaLogisticsRouteQueryRequest : ITopRequest<AlibabaLogisticsRouteQueryResponse>
    {
        /// <summary>
        /// 是否是空运的条件
        /// </summary>
        public Nullable<bool> AirTransport { get; set; }

        /// <summary>
        /// 过滤线路的公司塞选条件
        /// </summary>
        public string CorpList { get; set; }

        /// <summary>
        /// 目的地id，可以是市和区
        /// </summary>
        public Nullable<long> EndAreaId { get; set; }

        /// <summary>
        /// 是否合并线路
        /// </summary>
        public Nullable<bool> MergeRoute { get; set; }

        /// <summary>
        /// 当前第几页
        /// </summary>
        public Nullable<long> PageIndex { get; set; }

        /// <summary>
        /// 每页显示的线路数
        /// </summary>
        public Nullable<long> PageSize { get; set; }

        /// <summary>
        /// 线路是否具有代收货款服务
        /// </summary>
        public Nullable<bool> ShowCods { get; set; }

        /// <summary>
        /// 线路是否显示保障服务
        /// </summary>
        public Nullable<bool> ShowSpecials { get; set; }

        /// <summary>
        /// 是否在线路中显示评价和网点信息
        /// </summary>
        public Nullable<bool> ShowStatisticsInfo { get; set; }

        /// <summary>
        /// 线路排序方式。具体值如下,precise：精确匹配，corp：公司,wpa：重物价格升序,wpd：重物价格降序,vpa：体积价格升序,vpd：体积价格降序,trtid：运输时效降序,trtia：运输时效升序,corpLevel：公司级别，品牌>集市,evalScore：评价分数,routeTop：线路是否置顶,orderCount：下单量多少排序,special：保障服务优先排序。
        /// </summary>
        public string SortType { get; set; }

        /// <summary>
        /// 数据来源，默认开放部分物流公司。
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// 起始地id，可以是市和区
        /// </summary>
        public Nullable<long> StartAreaId { get; set; }

        /// <summary>
        /// 是否查询总的查询条件的公司数目
        /// </summary>
        public Nullable<bool> SummaryTotalCorps { get; set; }

        /// <summary>
        /// 是否统计对应公司的线路数
        /// </summary>
        public Nullable<bool> SummeryByCorp { get; set; }

        /// <summary>
        /// 如果查找不到指定地区的线路，是否对地址进行上翻。如杭州市滨江区的地址呗翻转为杭州市。
        /// </summary>
        public Nullable<bool> TurnLevel { get; set; }

        private IDictionary<string, string> otherParameters;

        #region ITopRequest Members

        public string GetApiName()
        {
            return "alibaba.logistics.route.query";
        }

        public IDictionary<string, string> GetParameters()
        {
            TopDictionary parameters = new TopDictionary();
            parameters.Add("air_transport", this.AirTransport);
            parameters.Add("corp_list", this.CorpList);
            parameters.Add("end_area_id", this.EndAreaId);
            parameters.Add("merge_route", this.MergeRoute);
            parameters.Add("page_index", this.PageIndex);
            parameters.Add("page_size", this.PageSize);
            parameters.Add("show_cods", this.ShowCods);
            parameters.Add("show_specials", this.ShowSpecials);
            parameters.Add("show_statistics_info", this.ShowStatisticsInfo);
            parameters.Add("sort_type", this.SortType);
            parameters.Add("source", this.Source);
            parameters.Add("start_area_id", this.StartAreaId);
            parameters.Add("summary_total_corps", this.SummaryTotalCorps);
            parameters.Add("summery_by_corp", this.SummeryByCorp);
            parameters.Add("turn_level", this.TurnLevel);
            parameters.AddAll(this.otherParameters);
            return parameters;
        }

        public void Validate()
        {
            RequestValidator.ValidateMaxListSize("corp_list", this.CorpList, 15);
            RequestValidator.ValidateRequired("end_area_id", this.EndAreaId);
            RequestValidator.ValidateRequired("page_index", this.PageIndex);
            RequestValidator.ValidateMaxValue("page_index", this.PageIndex, 100000);
            RequestValidator.ValidateMinValue("page_index", this.PageIndex, 1);
            RequestValidator.ValidateRequired("page_size", this.PageSize);
            RequestValidator.ValidateMaxValue("page_size", this.PageSize, 100);
            RequestValidator.ValidateMinValue("page_size", this.PageSize, 1);
            RequestValidator.ValidateRequired("start_area_id", this.StartAreaId);
        }

        #endregion

        public void AddOtherParameter(string key, string value)
        {
            if (this.otherParameters == null)
            {
                this.otherParameters = new TopDictionary();
            }
            this.otherParameters.Add(key, value);
        }
    }
}
