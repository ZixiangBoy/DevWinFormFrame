using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Top.Api.Domain
{
    /// <summary>
    /// QueryRouteResult Data Structure.
    /// </summary>
    [Serializable]
    public class QueryRouteResult : TopObject
    {
        /// <summary>
        /// 公司的线路统计信息。如德邦有几条线路。
        /// </summary>
        [XmlArray("company_route_summarys")]
        [XmlArrayItem("company_route_summary")]
        public List<CompanyRouteSummary> CompanyRouteSummarys { get; set; }

        /// <summary>
        /// 是否上翻
        /// </summary>
        [XmlElement("is_turn_level")]
        public bool IsTurnLevel { get; set; }

        /// <summary>
        /// 线路详情
        /// </summary>
        [XmlElement("pages_route_details")]
        public RouteAlpPage PagesRouteDetails { get; set; }

        /// <summary>
        /// 增值服务。
        /// </summary>
        [XmlArray("route_vas")]
        [XmlArrayItem("route_vas_info")]
        public List<RouteVasInfo> RouteVas { get; set; }

        /// <summary>
        /// 总公司数
        /// </summary>
        [XmlElement("total_corps")]
        public long TotalCorps { get; set; }

        /// <summary>
        /// 总线路数
        /// </summary>
        [XmlElement("total_routes")]
        public long TotalRoutes { get; set; }
    }
}
