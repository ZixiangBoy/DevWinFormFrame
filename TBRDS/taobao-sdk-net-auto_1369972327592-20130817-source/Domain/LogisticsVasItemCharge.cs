using System;
using System.Xml.Serialization;

namespace Top.Api.Domain
{
    /// <summary>
    /// LogisticsVasItemCharge Data Structure.
    /// </summary>
    [Serializable]
    public class LogisticsVasItemCharge : TopObject
    {
        /// <summary>
        /// 真实价格（折后价）
        /// </summary>
        [XmlElement("cost")]
        public string Cost { get; set; }

        /// <summary>
        /// 原价
        /// </summary>
        [XmlElement("original_cost")]
        public string OriginalCost { get; set; }

        /// <summary>
        /// 增值服务的code，为业务标识字段。
        /// </summary>
        [XmlElement("vas_code")]
        public string VasCode { get; set; }

        /// <summary>
        /// 增值服务系统字段，一般以业务字段vas_code为标识。
        /// </summary>
        [XmlElement("vas_id")]
        public string VasId { get; set; }
    }
}
