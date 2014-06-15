using System;
using System.Xml.Serialization;

namespace Top.Api.Domain
{
    /// <summary>
    /// LogisticsVas Data Structure.
    /// </summary>
    [Serializable]
    public class LogisticsVas : TopObject
    {
        /// <summary>
        /// 计费类型。0：计费；1：面议；2：免费
        /// </summary>
        [XmlElement("charge_calculate_type")]
        public string ChargeCalculateType { get; set; }

        /// <summary>
        /// 增值服务的描述
        /// </summary>
        [XmlElement("comments")]
        public string Comments { get; set; }

        /// <summary>
        /// 默认是否选中
        /// </summary>
        [XmlElement("default_selected")]
        public bool DefaultSelected { get; set; }

        /// <summary>
        /// 是否需要展示
        /// </summary>
        [XmlElement("displayable")]
        public bool Displayable { get; set; }

        /// <summary>
        /// 是否必选
        /// </summary>
        [XmlElement("needed")]
        public bool Needed { get; set; }

        /// <summary>
        /// 是否需要输入框，输入相应的金额。如报价金额。
        /// </summary>
        [XmlElement("value_displayable")]
        public bool ValueDisplayable { get; set; }

        /// <summary>
        /// 增值服务的code
        /// </summary>
        [XmlElement("vas_code")]
        public string VasCode { get; set; }

        /// <summary>
        /// 增值服务名
        /// </summary>
        [XmlElement("vas_name")]
        public string VasName { get; set; }
    }
}
