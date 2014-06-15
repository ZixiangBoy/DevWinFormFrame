using System;
using System.Xml.Serialization;

namespace Top.Api.Domain
{
    /// <summary>
    /// OrderCharge Data Structure.
    /// </summary>
    [Serializable]
    public class OrderCharge : TopObject
    {
        /// <summary>
        /// 原总费用
        /// </summary>
        [XmlElement("original_total_cost")]
        public string OriginalTotalCost { get; set; }

        /// <summary>
        /// 其他费用
        /// </summary>
        [XmlElement("other_cost")]
        public string OtherCost { get; set; }

        /// <summary>
        /// 总费用
        /// </summary>
        [XmlElement("total_cost")]
        public string TotalCost { get; set; }

        /// <summary>
        /// 节省费用
        /// </summary>
        [XmlElement("total_saved_cost")]
        public string TotalSavedCost { get; set; }

        /// <summary>
        /// 运输费用
        /// </summary>
        [XmlElement("transport_charge")]
        public TransportCharge TransportCharge { get; set; }

        /// <summary>
        /// 增值服务费用
        /// </summary>
        [XmlElement("vas_charge")]
        public LogisticsVasCharge VasCharge { get; set; }
    }
}
