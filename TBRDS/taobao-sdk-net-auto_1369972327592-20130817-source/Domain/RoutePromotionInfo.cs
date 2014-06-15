using System;
using System.Xml.Serialization;

namespace Top.Api.Domain
{
    /// <summary>
    /// RoutePromotionInfo Data Structure.
    /// </summary>
    [Serializable]
    public class RoutePromotionInfo : TopObject
    {
        /// <summary>
        /// 线路基本促销的id
        /// </summary>
        [XmlElement("base_promotion_id")]
        public string BasePromotionId { get; set; }

        /// <summary>
        /// 促销的描述
        /// </summary>
        [XmlElement("promotion_description")]
        public string PromotionDescription { get; set; }

        /// <summary>
        /// 具体活动详情页面链接
        /// </summary>
        [XmlElement("promotion_url")]
        public string PromotionUrl { get; set; }

        /// <summary>
        /// 是否包含非基础促销
        /// </summary>
        [XmlElement("unbase_promotion")]
        public bool UnbasePromotion { get; set; }
    }
}
