using System;
using System.Collections.Generic;
using Top.Api.Response;
using Top.Api.Util;

namespace Top.Api.Request
{
    /// <summary>
    /// TOP API: taobao.jushita.jdp.user.add
    /// </summary>
    public class JushitaJdpUserAddRequest : ITopRequest<JushitaJdpUserAddResponse>
    {
        /// <summary>
        /// 已废弃，使用页面中应用的配置。推送历史数据天数，只能为90天内，包含90天。  当此参数不填时，表示以页面中应用配置的历史天数为准；如果为0表示这个用户不推送历史数据；其它表示推送的历史天数。
        /// </summary>
        public Nullable<long> HistoryDays { get; set; }

        /// <summary>
        /// 已废弃，新版订单同步服务不要使用。同步用户数据的机器IP,必须是界面配置的IP。
        /// </summary>
        public string HostIp { get; set; }

        /// <summary>
        /// rds的实例名,3.0数据推送此参数必传
        /// </summary>
        public string RdsName { get; set; }

        /// <summary>
        /// 已废弃，使用页面中应用的配置。用户同步的数据类型,多个用','号分割。可选值：trade,refund,item
        /// </summary>
        public string Topics { get; set; }

        private IDictionary<string, string> otherParameters;

        #region ITopRequest Members

        public string GetApiName()
        {
            return "taobao.jushita.jdp.user.add";
        }

        public IDictionary<string, string> GetParameters()
        {
            TopDictionary parameters = new TopDictionary();
            parameters.Add("history_days", this.HistoryDays);
            parameters.Add("host_ip", this.HostIp);
            parameters.Add("rds_name", this.RdsName);
            parameters.Add("topics", this.Topics);
            parameters.AddAll(this.otherParameters);
            return parameters;
        }

        public void Validate()
        {
            RequestValidator.ValidateMaxValue("history_days", this.HistoryDays, 90);
            RequestValidator.ValidateMinValue("history_days", this.HistoryDays, 0);
            RequestValidator.ValidateMaxListSize("topics", this.Topics, 16);
        }

        #endregion

        public void AddOtherParameter(string key, string value)
        {
            if (this.otherParameters == null)
            {
                this.otherParameters = new TopDictionary();
            }
            this.otherParameters.Add(key, value);
        }
    }
}
