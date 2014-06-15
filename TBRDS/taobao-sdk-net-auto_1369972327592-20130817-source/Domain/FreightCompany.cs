using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Top.Api.Domain
{
    /// <summary>
    /// FreightCompany Data Structure.
    /// </summary>
    [Serializable]
    public class FreightCompany : TopObject
    {
        /// <summary>
        /// 物流公司描述
        /// </summary>
        [XmlElement("comments")]
        public string Comments { get; set; }

        /// <summary>
        /// 公司id
        /// </summary>
        [XmlElement("company_id")]
        public long CompanyId { get; set; }

        /// <summary>
        /// 物流公司名
        /// </summary>
        [XmlElement("company_name")]
        public string CompanyName { get; set; }

        /// <summary>
        /// 物流公司code标识
        /// </summary>
        [XmlElement("companye_code")]
        public string CompanyeCode { get; set; }

        /// <summary>
        /// 公司级别。auth：认证；brand：品牌；noAuth：未认证；normal：普通
        /// </summary>
        [XmlElement("corp_level")]
        public string CorpLevel { get; set; }

        /// <summary>
        /// 物流公司客服电话
        /// </summary>
        [XmlElement("customer_service_tel")]
        public string CustomerServiceTel { get; set; }

        /// <summary>
        /// 物流公司logo url
        /// </summary>
        [XmlElement("logo_url")]
        public string LogoUrl { get; set; }

        /// <summary>
        /// 物流公司店铺地址
        /// </summary>
        [XmlElement("shop_url")]
        public string ShopUrl { get; set; }

        /// <summary>
        /// 物流公司排序值
        /// </summary>
        [XmlElement("sort")]
        public long Sort { get; set; }

        /// <summary>
        /// 公司增值服务说明链接
        /// </summary>
        [XmlElement("vas_fee_help_url")]
        public string VasFeeHelpUrl { get; set; }

        /// <summary>
        /// 旺旺列表
        /// </summary>
        [XmlArray("wangwang_list")]
        [XmlArrayItem("wangwang_info")]
        public List<WangwangInfo> WangwangList { get; set; }
    }
}
