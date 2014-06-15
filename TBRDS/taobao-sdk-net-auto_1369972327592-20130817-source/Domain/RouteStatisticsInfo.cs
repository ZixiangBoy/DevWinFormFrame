using System;
using System.Xml.Serialization;

namespace Top.Api.Domain
{
    /// <summary>
    /// RouteStatisticsInfo Data Structure.
    /// </summary>
    [Serializable]
    public class RouteStatisticsInfo : TopObject
    {
        /// <summary>
        /// 评价数
        /// </summary>
        [XmlElement("evaluation_count")]
        public long EvaluationCount { get; set; }

        /// <summary>
        /// 评价分值
        /// </summary>
        [XmlElement("evaluation_score")]
        public string EvaluationScore { get; set; }

        /// <summary>
        /// 发货网点个数
        /// </summary>
        [XmlElement("from_network_count")]
        public long FromNetworkCount { get; set; }

        /// <summary>
        /// 目的地网点个数
        /// </summary>
        [XmlElement("to_network_count")]
        public long ToNetworkCount { get; set; }

        /// <summary>
        /// 干线上的下单量
        /// </summary>
        [XmlElement("trunk_route_order_count")]
        public long TrunkRouteOrderCount { get; set; }
    }
}
