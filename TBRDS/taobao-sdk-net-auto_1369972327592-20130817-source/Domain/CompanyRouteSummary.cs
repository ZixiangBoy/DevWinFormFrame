using System;
using System.Xml.Serialization;

namespace Top.Api.Domain
{
    /// <summary>
    /// CompanyRouteSummary Data Structure.
    /// </summary>
    [Serializable]
    public class CompanyRouteSummary : TopObject
    {
        /// <summary>
        /// 物流公司code
        /// </summary>
        [XmlElement("company_code")]
        public string CompanyCode { get; set; }

        /// <summary>
        /// 物流公司id
        /// </summary>
        [XmlElement("company_id")]
        public string CompanyId { get; set; }

        /// <summary>
        /// 物流公司名
        /// </summary>
        [XmlElement("company_name")]
        public string CompanyName { get; set; }

        /// <summary>
        /// 公司的线路数目
        /// </summary>
        [XmlElement("route_counts")]
        public long RouteCounts { get; set; }
    }
}
