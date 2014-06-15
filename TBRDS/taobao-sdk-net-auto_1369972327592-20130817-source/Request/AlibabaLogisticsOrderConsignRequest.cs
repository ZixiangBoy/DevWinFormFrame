using System;
using System.Collections.Generic;
using Top.Api.Response;
using Top.Api.Util;

namespace Top.Api.Request
{
    /// <summary>
    /// TOP API: alibaba.logistics.order.consign
    /// </summary>
    public class AlibabaLogisticsOrderConsignRequest : ITopRequest<AlibabaLogisticsOrderConsignResponse>
    {
        /// <summary>
        /// 货物描述
        /// </summary>
        public string CargoDescription { get; set; }

        /// <summary>
        /// 货物名称
        /// </summary>
        public string CargoName { get; set; }

        /// <summary>
        /// 物流订单id
        /// </summary>
        public Nullable<long> OrderId { get; set; }

        /// <summary>
        /// 付款方式。0：发货人支付；1：收货人支付；2：双方支付
        /// </summary>
        public string PayType { get; set; }

        /// <summary>
        /// 收件人地址
        /// </summary>
        public string ReceiverAddress { get; set; }

        /// <summary>
        /// 收件人地区编码
        /// </summary>
        public Nullable<long> ReceiverAreaId { get; set; }

        /// <summary>
        /// 收件人城市名
        /// </summary>
        public string ReceiverCityName { get; set; }

        /// <summary>
        /// 收件人公司名称
        /// </summary>
        public string ReceiverCorpName { get; set; }

        /// <summary>
        /// 收件人区名
        /// </summary>
        public string ReceiverCountyName { get; set; }

        /// <summary>
        /// 收件人手机号
        /// </summary>
        public string ReceiverMobile { get; set; }

        /// <summary>
        /// 收件人名
        /// </summary>
        public string ReceiverName { get; set; }

        /// <summary>
        /// 收件人电话区号
        /// </summary>
        public string ReceiverPhoneAreaCode { get; set; }

        /// <summary>
        /// 收件人电话号码
        /// </summary>
        public string ReceiverPhoneTel { get; set; }

        /// <summary>
        /// 收件人电话分机号
        /// </summary>
        public string ReceiverPhoneTelExt { get; set; }

        /// <summary>
        /// 收件人邮编
        /// </summary>
        public string ReceiverPostcode { get; set; }

        /// <summary>
        /// 收件人省名
        /// </summary>
        public string ReceiverProvinceName { get; set; }

        /// <summary>
        /// 收件人旺旺号
        /// </summary>
        public string ReceiverWangwangNo { get; set; }

        /// <summary>
        /// 退货接收人地址
        /// </summary>
        public string RefunderAddress { get; set; }

        /// <summary>
        /// 退货接收人地区id
        /// </summary>
        public Nullable<long> RefunderAreaId { get; set; }

        /// <summary>
        /// 退货接收人城市名
        /// </summary>
        public string RefunderCityName { get; set; }

        /// <summary>
        /// 退货接收人公司名称
        /// </summary>
        public string RefunderCorpName { get; set; }

        /// <summary>
        /// 退货接收人区名
        /// </summary>
        public string RefunderCountyName { get; set; }

        /// <summary>
        /// 退货接收人手机号码
        /// </summary>
        public string RefunderMobile { get; set; }

        /// <summary>
        /// 退货接收人姓名
        /// </summary>
        public string RefunderName { get; set; }

        /// <summary>
        /// 退货接收人电话区号
        /// </summary>
        public string RefunderPhoneAreaCode { get; set; }

        /// <summary>
        /// 退货接收人电话号码
        /// </summary>
        public string RefunderPhoneTel { get; set; }

        /// <summary>
        /// 退货接收人电话分机号
        /// </summary>
        public string RefunderPhoneTelExt { get; set; }

        /// <summary>
        /// 退货接收人邮编
        /// </summary>
        public string RefunderPostcode { get; set; }

        /// <summary>
        /// 退货接收人省名
        /// </summary>
        public string RefunderProvinceName { get; set; }

        /// <summary>
        /// 退货接收人旺旺id
        /// </summary>
        public string RefunderWangwangNo { get; set; }

        /// <summary>
        /// 发货备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 线路code，线路的业务标识。
        /// </summary>
        public string RouteCode { get; set; }

        /// <summary>
        /// 发货人地址
        /// </summary>
        public string SenderAddress { get; set; }

        /// <summary>
        /// 发货地区编码
        /// </summary>
        public Nullable<long> SenderAreaId { get; set; }

        /// <summary>
        /// 发货人城市名
        /// </summary>
        public string SenderCityName { get; set; }

        /// <summary>
        /// 发货人公司名称
        /// </summary>
        public string SenderCorpName { get; set; }

        /// <summary>
        /// 发货人区名
        /// </summary>
        public string SenderCountyName { get; set; }

        /// <summary>
        /// 发货人手机号
        /// </summary>
        public string SenderMobile { get; set; }

        /// <summary>
        /// 发货人姓名
        /// </summary>
        public string SenderName { get; set; }

        /// <summary>
        /// 发货人电话区号
        /// </summary>
        public string SenderPhoneAreaCode { get; set; }

        /// <summary>
        /// 发货人电话
        /// </summary>
        public string SenderPhoneTel { get; set; }

        /// <summary>
        /// 发货人电话分机号
        /// </summary>
        public string SenderPhoneTelExt { get; set; }

        /// <summary>
        /// 发货人地区邮编
        /// </summary>
        public string SenderPostcode { get; set; }

        /// <summary>
        /// 发货人省名
        /// </summary>
        public string SenderProvinceName { get; set; }

        /// <summary>
        /// 发货人旺旺号
        /// </summary>
        public string SenderWangwangNo { get; set; }

        /// <summary>
        /// top开放的来源。默认都使用：TAOBAO_TOP。
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// 货物件数
        /// </summary>
        public Nullable<long> TotalNumber { get; set; }

        /// <summary>
        /// 货物体积
        /// </summary>
        public string TotalVolume { get; set; }

        /// <summary>
        /// 货物重量
        /// </summary>
        public string TotalWeight { get; set; }

        /// <summary>
        /// 下单选中的增值服务
        /// </summary>
        public string VasList { get; set; }

        private IDictionary<string, string> otherParameters;

        #region ITopRequest Members

        public string GetApiName()
        {
            return "alibaba.logistics.order.consign";
        }

        public IDictionary<string, string> GetParameters()
        {
            TopDictionary parameters = new TopDictionary();
            parameters.Add("cargo_description", this.CargoDescription);
            parameters.Add("cargo_name", this.CargoName);
            parameters.Add("order_id", this.OrderId);
            parameters.Add("pay_type", this.PayType);
            parameters.Add("receiver_address", this.ReceiverAddress);
            parameters.Add("receiver_area_id", this.ReceiverAreaId);
            parameters.Add("receiver_city_name", this.ReceiverCityName);
            parameters.Add("receiver_corp_name", this.ReceiverCorpName);
            parameters.Add("receiver_county_name", this.ReceiverCountyName);
            parameters.Add("receiver_mobile", this.ReceiverMobile);
            parameters.Add("receiver_name", this.ReceiverName);
            parameters.Add("receiver_phone_area_code", this.ReceiverPhoneAreaCode);
            parameters.Add("receiver_phone_tel", this.ReceiverPhoneTel);
            parameters.Add("receiver_phone_tel_ext", this.ReceiverPhoneTelExt);
            parameters.Add("receiver_postcode", this.ReceiverPostcode);
            parameters.Add("receiver_province_name", this.ReceiverProvinceName);
            parameters.Add("receiver_wangwang_no", this.ReceiverWangwangNo);
            parameters.Add("refunder_address", this.RefunderAddress);
            parameters.Add("refunder_area_id", this.RefunderAreaId);
            parameters.Add("refunder_city_name", this.RefunderCityName);
            parameters.Add("refunder_corp_name", this.RefunderCorpName);
            parameters.Add("refunder_county_name", this.RefunderCountyName);
            parameters.Add("refunder_mobile", this.RefunderMobile);
            parameters.Add("refunder_name", this.RefunderName);
            parameters.Add("refunder_phone_area_code", this.RefunderPhoneAreaCode);
            parameters.Add("refunder_phone_tel", this.RefunderPhoneTel);
            parameters.Add("refunder_phone_tel_ext", this.RefunderPhoneTelExt);
            parameters.Add("refunder_postcode", this.RefunderPostcode);
            parameters.Add("refunder_province_name", this.RefunderProvinceName);
            parameters.Add("refunder_wangwang_no", this.RefunderWangwangNo);
            parameters.Add("remark", this.Remark);
            parameters.Add("route_code", this.RouteCode);
            parameters.Add("sender_address", this.SenderAddress);
            parameters.Add("sender_area_id", this.SenderAreaId);
            parameters.Add("sender_city_name", this.SenderCityName);
            parameters.Add("sender_corp_name", this.SenderCorpName);
            parameters.Add("sender_county_name", this.SenderCountyName);
            parameters.Add("sender_mobile", this.SenderMobile);
            parameters.Add("sender_name", this.SenderName);
            parameters.Add("sender_phone_area_code", this.SenderPhoneAreaCode);
            parameters.Add("sender_phone_tel", this.SenderPhoneTel);
            parameters.Add("sender_phone_tel_ext", this.SenderPhoneTelExt);
            parameters.Add("sender_postcode", this.SenderPostcode);
            parameters.Add("sender_province_name", this.SenderProvinceName);
            parameters.Add("sender_wangwang_no", this.SenderWangwangNo);
            parameters.Add("source", this.Source);
            parameters.Add("total_number", this.TotalNumber);
            parameters.Add("total_volume", this.TotalVolume);
            parameters.Add("total_weight", this.TotalWeight);
            parameters.Add("vas_list", this.VasList);
            parameters.AddAll(this.otherParameters);
            return parameters;
        }

        public void Validate()
        {
            RequestValidator.ValidateRequired("cargo_name", this.CargoName);
            RequestValidator.ValidateRequired("order_id", this.OrderId);
            RequestValidator.ValidateRequired("pay_type", this.PayType);
            RequestValidator.ValidateRequired("route_code", this.RouteCode);
            RequestValidator.ValidateRequired("source", this.Source);
            RequestValidator.ValidateRequired("total_number", this.TotalNumber);
            RequestValidator.ValidateMaxListSize("vas_list", this.VasList, 12);
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
