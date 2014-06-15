using System;
using System.Xml.Serialization;

namespace Top.Api.Domain
{
    /// <summary>
    /// CancelOrderResult Data Structure.
    /// </summary>
    [Serializable]
    public class CancelOrderResult : TopObject
    {
        /// <summary>
        /// 撤销后重新创建的订单的订单号
        /// </summary>
        [XmlElement("recreate_order_id")]
        public long RecreateOrderId { get; set; }
    }
}
