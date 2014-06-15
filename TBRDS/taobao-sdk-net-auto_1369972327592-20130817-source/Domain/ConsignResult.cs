using System;
using System.Xml.Serialization;

namespace Top.Api.Domain
{
    /// <summary>
    /// ConsignResult Data Structure.
    /// </summary>
    [Serializable]
    public class ConsignResult : TopObject
    {
        /// <summary>
        /// 物流编号，作为业务标识字段。
        /// </summary>
        [XmlElement("logistics_id")]
        public string LogisticsId { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        [XmlElement("order_id")]
        public long OrderId { get; set; }
    }
}
