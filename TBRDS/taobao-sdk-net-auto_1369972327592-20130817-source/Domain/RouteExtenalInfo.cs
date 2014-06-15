using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Top.Api.Domain
{
    /// <summary>
    /// RouteExtenalInfo Data Structure.
    /// </summary>
    [Serializable]
    public class RouteExtenalInfo : TopObject
    {
        /// <summary>
        /// 线路是否支持代收货款
        /// </summary>
        [XmlElement("cod")]
        public bool Cod { get; set; }

        /// <summary>
        /// 是否缴纳保证金
        /// </summary>
        [XmlElement("credit_opened")]
        public bool CreditOpened { get; set; }

        /// <summary>
        /// 缴纳保证金额
        /// </summary>
        [XmlElement("credit_total_balance")]
        public string CreditTotalBalance { get; set; }

        /// <summary>
        /// 线路为推荐线路
        /// </summary>
        [XmlElement("recommend")]
        public bool Recommend { get; set; }

        /// <summary>
        /// 支持的保障服务类型列表。JGZS：价格真实；SXBZ：时效保障；KSSL：快速手里。
        /// </summary>
        [XmlArray("special_codes")]
        [XmlArrayItem("string")]
        public List<string> SpecialCodes { get; set; }

        /// <summary>
        /// 是否置顶
        /// </summary>
        [XmlElement("top")]
        public bool Top { get; set; }
    }
}
