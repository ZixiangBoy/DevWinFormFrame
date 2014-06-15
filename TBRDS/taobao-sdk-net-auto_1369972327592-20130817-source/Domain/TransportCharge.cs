using System;
using System.Xml.Serialization;

namespace Top.Api.Domain
{
    /// <summary>
    /// TransportCharge Data Structure.
    /// </summary>
    [Serializable]
    public class TransportCharge : TopObject
    {
        /// <summary>
        /// 总运费
        /// </summary>
        [XmlElement("cost")]
        public string Cost { get; set; }

        /// <summary>
        /// 计费方式。by_weight:按重量；by_volume:按体积。
        /// </summary>
        [XmlElement("cost_by")]
        public string CostBy { get; set; }

        /// <summary>
        /// 运费原价
        /// </summary>
        [XmlElement("original_cost")]
        public string OriginalCost { get; set; }

        /// <summary>
        /// 节省费用
        /// </summary>
        [XmlElement("saved_cost")]
        public string SavedCost { get; set; }
    }
}
