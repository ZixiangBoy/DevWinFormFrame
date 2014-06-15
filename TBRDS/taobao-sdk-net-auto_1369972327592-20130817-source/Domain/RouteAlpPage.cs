using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Top.Api.Domain
{
    /// <summary>
    /// RouteAlpPage Data Structure.
    /// </summary>
    [Serializable]
    public class RouteAlpPage : TopObject
    {
        /// <summary>
        /// 线路的列表
        /// </summary>
        [XmlArray("datas")]
        [XmlArrayItem("complex_logistics_route")]
        public List<ComplexLogisticsRoute> Datas { get; set; }

        /// <summary>
        /// 结束记录数
        /// </summary>
        [XmlElement("end")]
        public long End { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        [XmlElement("page_count")]
        public long PageCount { get; set; }

        /// <summary>
        /// 当前页码
        /// </summary>
        [XmlElement("page_index")]
        public long PageIndex { get; set; }

        /// <summary>
        /// 每页条数
        /// </summary>
        [XmlElement("page_size")]
        public long PageSize { get; set; }

        /// <summary>
        /// 总的记录数
        /// </summary>
        [XmlElement("record_count")]
        public long RecordCount { get; set; }

        /// <summary>
        /// 开始记录数
        /// </summary>
        [XmlElement("start")]
        public long Start { get; set; }
    }
}
