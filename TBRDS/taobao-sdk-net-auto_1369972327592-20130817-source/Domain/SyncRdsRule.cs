using System;
using System.Xml.Serialization;

namespace Top.Api.Domain
{
    /// <summary>
    /// SyncRdsRule Data Structure.
    /// </summary>
    [Serializable]
    public class SyncRdsRule : TopObject
    {
        /// <summary>
        /// rds数据库实例名称
        /// </summary>
        [XmlElement("instance_name")]
        public string InstanceName { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        [XmlElement("user_nick")]
        public string UserNick { get; set; }
    }
}
