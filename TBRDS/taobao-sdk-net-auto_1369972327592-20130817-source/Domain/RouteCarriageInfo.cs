using System;
using System.Xml.Serialization;

namespace Top.Api.Domain
{
    /// <summary>
    /// RouteCarriageInfo Data Structure.
    /// </summary>
    [Serializable]
    public class RouteCarriageInfo : TopObject
    {
        /// <summary>
        /// 续重
        /// </summary>
        [XmlElement("add_fee")]
        public string AddFee { get; set; }

        /// <summary>
        /// 线路的备注
        /// </summary>
        [XmlElement("comments")]
        public string Comments { get; set; }

        /// <summary>
        /// 送货时效
        /// </summary>
        [XmlElement("give_time")]
        public string GiveTime { get; set; }

        /// <summary>
        /// 首重
        /// </summary>
        [XmlElement("initial_fee")]
        public string InitialFee { get; set; }

        /// <summary>
        /// 保底消费
        /// </summary>
        [XmlElement("least_expense")]
        public string LeastExpense { get; set; }

        /// <summary>
        /// 轻货原价（按体积计费）
        /// </summary>
        [XmlElement("orig_volume_rate")]
        public string OrigVolumeRate { get; set; }

        /// <summary>
        /// 重物原价
        /// </summary>
        [XmlElement("orig_weight_rate")]
        public string OrigWeightRate { get; set; }

        /// <summary>
        /// 价格描述
        /// </summary>
        [XmlElement("price_description")]
        public string PriceDescription { get; set; }

        /// <summary>
        /// 提货时间
        /// </summary>
        [XmlElement("take_time")]
        public string TakeTime { get; set; }

        /// <summary>
        /// 运价模式。D2D:门到门；S2S：站到站；D2S：门到站；S2D：站到门。
        /// </summary>
        [XmlElement("transport_mode")]
        public string TransportMode { get; set; }

        /// <summary>
        /// 运输方式名称
        /// </summary>
        [XmlElement("transport_name")]
        public string TransportName { get; set; }

        /// <summary>
        /// 运输时效的文字描述
        /// </summary>
        [XmlElement("transport_time")]
        public string TransportTime { get; set; }

        /// <summary>
        /// 运输时效的小时数。可用于排序。
        /// </summary>
        [XmlElement("transport_time_hours")]
        public long TransportTimeHours { get; set; }

        /// <summary>
        /// 运输方式code标识
        /// </summary>
        [XmlElement("transport_type_code")]
        public string TransportTypeCode { get; set; }

        /// <summary>
        /// 运输方式。QC：汽运；HK:航空。
        /// </summary>
        [XmlElement("transport_way")]
        public string TransportWay { get; set; }

        /// <summary>
        /// 轻货价格（按体积计费）
        /// </summary>
        [XmlElement("volume_rate")]
        public string VolumeRate { get; set; }

        /// <summary>
        /// 重物价格
        /// </summary>
        [XmlElement("weight_rate")]
        public string WeightRate { get; set; }
    }
}
