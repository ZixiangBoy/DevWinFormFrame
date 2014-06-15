using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Top.Api.Domain
{
    /// <summary>
    /// LogisticsVasCharge Data Structure.
    /// </summary>
    [Serializable]
    public class LogisticsVasCharge : TopObject
    {
        /// <summary>
        /// 原始总费用
        /// </summary>
        [XmlElement("original_total_vas_cost")]
        public string OriginalTotalVasCost { get; set; }

        /// <summary>
        /// 总费用
        /// </summary>
        [XmlElement("total_vas_cost")]
        public string TotalVasCost { get; set; }

        /// <summary>
        /// 节省的费用。即原始费用-现在费用
        /// </summary>
        [XmlElement("total_vas_save_cost")]
        public string TotalVasSaveCost { get; set; }

        /// <summary>
        /// 增值服务每项费用
        /// </summary>
        [XmlArray("vas_cost_list")]
        [XmlArrayItem("logistics_vas_item_charge")]
        public List<LogisticsVasItemCharge> VasCostList { get; set; }
    }
}
