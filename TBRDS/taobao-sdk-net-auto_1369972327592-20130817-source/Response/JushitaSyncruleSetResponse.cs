using System;
using System.Xml.Serialization;

namespace Top.Api.Response
{
    /// <summary>
    /// JushitaSyncruleSetResponse.
    /// </summary>
    public class JushitaSyncruleSetResponse : TopResponse
    {
        /// <summary>
        /// 返回添加推送规则成功的用户nicks
        /// </summary>
        [XmlElement("result")]
        public string Result { get; set; }
    }
}
