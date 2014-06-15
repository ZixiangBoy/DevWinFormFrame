using System;
using System.Xml.Serialization;

namespace Top.Api.Domain
{
    /// <summary>
    /// WangwangInfo Data Structure.
    /// </summary>
    [Serializable]
    public class WangwangInfo : TopObject
    {
        /// <summary>
        /// 旺旺类型。cntaobao：taobao旺旺；cnalichn：阿里巴巴旺旺。
        /// </summary>
        [XmlElement("site")]
        public string Site { get; set; }

        /// <summary>
        /// 旺旺id
        /// </summary>
        [XmlElement("wangwang_id")]
        public string WangwangId { get; set; }
    }
}
