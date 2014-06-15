using System;
using System.Collections.Generic;
using Top.Api.Response;
using Top.Api.Util;

namespace Top.Api.Request
{
    /// <summary>
    /// TOP API: taobao.jushita.syncrule.query
    /// </summary>
    public class JushitaSyncruleQueryRequest : ITopRequest<JushitaSyncruleQueryResponse>
    {
        /// <summary>
        /// ISV购买的rds实例名称
        /// </summary>
        public string InstanceName { get; set; }

        /// <summary>
        /// 传入的用户昵称，多个以','分隔,一次传入最大支持500个。
        /// </summary>
        public string UserNicks { get; set; }

        private IDictionary<string, string> otherParameters;

        #region ITopRequest Members

        public string GetApiName()
        {
            return "taobao.jushita.syncrule.query";
        }

        public IDictionary<string, string> GetParameters()
        {
            TopDictionary parameters = new TopDictionary();
            parameters.Add("instance_name", this.InstanceName);
            parameters.Add("user_nicks", this.UserNicks);
            parameters.AddAll(this.otherParameters);
            return parameters;
        }

        public void Validate()
        {
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
