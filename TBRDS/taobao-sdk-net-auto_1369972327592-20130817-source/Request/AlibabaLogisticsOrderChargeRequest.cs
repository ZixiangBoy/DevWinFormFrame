using System;
using System.Collections.Generic;
using Top.Api.Response;
using Top.Api.Util;

namespace Top.Api.Request
{
    /// <summary>
    /// TOP API: alibaba.logistics.order.charge
    /// </summary>
    public class AlibabaLogisticsOrderChargeRequest : ITopRequest<AlibabaLogisticsOrderChargeResponse>
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
        /// 付款方式。0：发货人支付；1：收货人支付；2：双方支付
        /// </summary>
        public string PayType { get; set; }

        /// <summary>
        /// 线路标志
        /// </summary>
        public string RouteCode { get; set; }

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
            return "alibaba.logistics.order.charge";
        }

        public IDictionary<string, string> GetParameters()
        {
            TopDictionary parameters = new TopDictionary();
            parameters.Add("cargo_description", this.CargoDescription);
            parameters.Add("cargo_name", this.CargoName);
            parameters.Add("pay_type", this.PayType);
            parameters.Add("route_code", this.RouteCode);
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
            RequestValidator.ValidateRequired("pay_type", this.PayType);
            RequestValidator.ValidateRequired("route_code", this.RouteCode);
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
