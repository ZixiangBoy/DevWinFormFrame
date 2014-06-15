using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Top.Api.Domain
{
    /// <summary>
    /// RouteVasInfo Data Structure.
    /// </summary>
    [Serializable]
    public class RouteVasInfo : TopObject
    {
        /// <summary>
        /// 线路code标志
        /// </summary>
        [XmlElement("route_code")]
        public string RouteCode { get; set; }

        /// <summary>
        /// 增值服务列表
        /// </summary>
        [XmlArray("vas_list")]
        [XmlArrayItem("logistics_vas")]
        public List<LogisticsVas> VasList { get; set; }
    }
}
