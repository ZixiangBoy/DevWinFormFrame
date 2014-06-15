using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Top.Api.Domain;

namespace Top.Api.Response
{
    /// <summary>
    /// JushitaSyncruleQueryResponse.
    /// </summary>
    public class JushitaSyncruleQueryResponse : TopResponse
    {
        /// <summary>
        /// 返回的结果是一个推送规则的列表
        /// </summary>
        [XmlArray("syncrulelist")]
        [XmlArrayItem("sync_rds_rule")]
        public List<SyncRdsRule> Syncrulelist { get; set; }
    }
}
