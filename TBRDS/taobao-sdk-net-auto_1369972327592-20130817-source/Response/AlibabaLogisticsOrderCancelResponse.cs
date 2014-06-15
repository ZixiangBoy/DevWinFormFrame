using System;
using System.Xml.Serialization;
using Top.Api.Domain;

namespace Top.Api.Response
{
    /// <summary>
    /// AlibabaLogisticsOrderCancelResponse.
    /// </summary>
    public class AlibabaLogisticsOrderCancelResponse : TopResponse
    {
        /// <summary>
        /// 撤销物流订单结果
        /// </summary>
        [XmlElement("cancel_order_result")]
        public CancelOrderResult CancelOrderResult { get; set; }
    }
}
