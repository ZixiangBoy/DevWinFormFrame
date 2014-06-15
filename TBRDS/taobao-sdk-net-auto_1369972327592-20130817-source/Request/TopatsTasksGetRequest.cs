using System;
using System.Collections.Generic;
using Top.Api.Response;
using Top.Api.Util;

namespace Top.Api.Request
{
    /// <summary>
    /// TOP API: taobao.topats.tasks.get
    /// </summary>
    public class TopatsTasksGetRequest : ITopRequest<TopatsTasksGetResponse>
    {
        /// <summary>
        /// 要查询的已经创建的定时任务的结束时间(这里的时间是指执行时间)。
        /// </summary>
        public Nullable<DateTime> EndTime { get; set; }

        /// <summary>
        /// 要查询的已创建过的定时任务的开始时间(这里的时间是指执行时间)。
        /// </summary>
        public Nullable<DateTime> StartTime { get; set; }

        private IDictionary<string, string> otherParameters;

        #region ITopRequest Members

        public string GetApiName()
        {
            return "taobao.topats.tasks.get";
        }

        public IDictionary<string, string> GetParameters()
        {
            TopDictionary parameters = new TopDictionary();
            parameters.Add("end_time", this.EndTime);
            parameters.Add("start_time", this.StartTime);
            parameters.AddAll(this.otherParameters);
            return parameters;
        }

        public void Validate()
        {
            RequestValidator.ValidateRequired("end_time", this.EndTime);
            RequestValidator.ValidateRequired("start_time", this.StartTime);
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
