using System;
using System.Collections.Generic;
using Top.Api.Response;
using Top.Api.Util;

namespace Top.Api.Request
{
    /// <summary>
    /// TOP API: taobao.jushita.syncrule.set
    /// </summary>
    public class JushitaSyncruleSetRequest : ITopRequest<JushitaSyncruleSetResponse>
    {
        /// <summary>
        /// ISV购买的rds数据库的实例名称
        /// </summary>
        public string InstanceName { get; set; }

        /// <summary>
        /// userNicks为传入的用户昵称字符串，多个用户可以使用','进行分隔，一次最大支持20个
        /// </summary>
        public string UserNicks { get; set; }

        private IDictionary<string, string> otherParameters;

        #region ITopRequest Members

        public string GetApiName()
        {
            return "taobao.jushita.syncrule.set";
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
            RequestValidator.ValidateRequired("instance_name", this.InstanceName);
            RequestValidator.ValidateRequired("user_nicks", this.UserNicks);
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
