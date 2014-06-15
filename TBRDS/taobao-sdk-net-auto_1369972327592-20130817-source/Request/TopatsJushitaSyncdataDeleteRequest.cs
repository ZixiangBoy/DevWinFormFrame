using System;
using System.Collections.Generic;
using Top.Api.Response;
using Top.Api.Util;

namespace Top.Api.Request
{
    /// <summary>
    /// TOP API: taobao.topats.jushita.syncdata.delete
    /// </summary>
    public class TopatsJushitaSyncdataDeleteRequest : ITopRequest<TopatsJushitaSyncdataDeleteResponse>
    {
        /// <summary>
        /// 删除数据时间段的结束修改时间，格式为：yyyy-MM-dd HH:mm:ss，结束时间必须为前天的23:59:59秒以前。
        /// </summary>
        public Nullable<DateTime> EndDate { get; set; }

        /// <summary>
        /// 删除数据时间段的起始修改时间，格式为：yyyy-MM-dd HH:mm:ss
        /// </summary>
        public Nullable<DateTime> StartDate { get; set; }

        /// <summary>
        /// 推送的数据类型，可选值为：item, trade, refund，同时删除多种类型以分号分隔，如："item;trade;refund"
        /// </summary>
        public string SyncType { get; set; }

        /// <summary>
        /// 用户昵称，不填表示删除所有用户的数据。
        /// </summary>
        public string UserNick { get; set; }

        private IDictionary<string, string> otherParameters;

        #region ITopRequest Members

        public string GetApiName()
        {
            return "taobao.topats.jushita.syncdata.delete";
        }

        public IDictionary<string, string> GetParameters()
        {
            TopDictionary parameters = new TopDictionary();
            parameters.Add("end_date", this.EndDate);
            parameters.Add("start_date", this.StartDate);
            parameters.Add("sync_type", this.SyncType);
            parameters.Add("user_nick", this.UserNick);
            parameters.AddAll(this.otherParameters);
            return parameters;
        }

        public void Validate()
        {
            RequestValidator.ValidateRequired("end_date", this.EndDate);
            RequestValidator.ValidateRequired("start_date", this.StartDate);
            RequestValidator.ValidateRequired("sync_type", this.SyncType);
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
