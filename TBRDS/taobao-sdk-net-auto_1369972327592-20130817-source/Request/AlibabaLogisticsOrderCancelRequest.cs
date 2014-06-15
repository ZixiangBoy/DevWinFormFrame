using System;
using System.Collections.Generic;
using Top.Api.Response;
using Top.Api.Util;

namespace Top.Api.Request
{
    /// <summary>
    /// TOP API: alibaba.logistics.order.cancel
    /// </summary>
    public class AlibabaLogisticsOrderCancelRequest : ITopRequest<AlibabaLogisticsOrderCancelResponse>
    {
        /// <summary>
        /// 物流订单id
        /// </summary>
        public Nullable<long> OrderId { get; set; }

        /// <summary>
        /// 撤销原因说明。
        /// </summary>
        public string Reason { get; set; }

        private IDictionary<string, string> otherParameters;

        #region ITopRequest Members

        public string GetApiName()
        {
            return "alibaba.logistics.order.cancel";
        }

        public IDictionary<string, string> GetParameters()
        {
            TopDictionary parameters = new TopDictionary();
            parameters.Add("order_id", this.OrderId);
            parameters.Add("reason", this.Reason);
            parameters.AddAll(this.otherParameters);
            return parameters;
        }

        public void Validate()
        {
            RequestValidator.ValidateRequired("order_id", this.OrderId);
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
