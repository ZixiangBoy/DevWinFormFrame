using System;
using System.Xml.Serialization;

namespace Top.Api.Response
{
    /// <summary>
    /// JushitaJdpUserInvalidResponse.
    /// </summary>
    public class JushitaJdpUserInvalidResponse : TopResponse
    {
        /// <summary>
        /// 是否删除成功
        /// </summary>
        [XmlElement("is_success")]
        public bool IsSuccess { get; set; }
    }
}
