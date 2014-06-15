using System;
using System.Xml.Serialization;

namespace Top.Api.Domain
{
    /// <summary>
    /// ComplexLogisticsRoute Data Structure.
    /// </summary>
    [Serializable]
    public class ComplexLogisticsRoute : TopObject
    {
        /// <summary>
        /// 线路运输相关的信息
        /// </summary>
        [XmlElement("carriage_info")]
        public RouteCarriageInfo CarriageInfo { get; set; }

        /// <summary>
        /// 物流公司信息
        /// </summary>
        [XmlElement("company")]
        public FreightCompany Company { get; set; }

        /// <summary>
        /// 线路扩展信息
        /// </summary>
        [XmlElement("extenal_info")]
        public RouteExtenalInfo ExtenalInfo { get; set; }

        /// <summary>
        /// 始发地area id
        /// </summary>
        [XmlElement("from_area_id")]
        public long FromAreaId { get; set; }

        /// <summary>
        /// 始发城市名
        /// </summary>
        [XmlElement("from_city_name")]
        public string FromCityName { get; set; }

        /// <summary>
        /// 始发区
        /// </summary>
        [XmlElement("from_county_name")]
        public string FromCountyName { get; set; }

        /// <summary>
        /// 始发省
        /// </summary>
        [XmlElement("from_province_name")]
        public string FromProvinceName { get; set; }

        /// <summary>
        /// 促销相关信息
        /// </summary>
        [XmlElement("promotion_info")]
        public RoutePromotionInfo PromotionInfo { get; set; }

        /// <summary>
        /// 线路code标识
        /// </summary>
        [XmlElement("route_code")]
        public string RouteCode { get; set; }

        /// <summary>
        /// 线路上相关的统计信息
        /// </summary>
        [XmlElement("statistics_info")]
        public RouteStatisticsInfo StatisticsInfo { get; set; }

        /// <summary>
        /// 目的地area id
        /// </summary>
        [XmlElement("to_area_id")]
        public long ToAreaId { get; set; }

        /// <summary>
        /// 目的地城市
        /// </summary>
        [XmlElement("to_city_name")]
        public string ToCityName { get; set; }

        /// <summary>
        /// 目的地区
        /// </summary>
        [XmlElement("to_county_name")]
        public string ToCountyName { get; set; }

        /// <summary>
        /// 目的地省
        /// </summary>
        [XmlElement("to_province_name")]
        public string ToProvinceName { get; set; }
    }
}
