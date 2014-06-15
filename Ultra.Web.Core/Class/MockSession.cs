using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Ultra.Web.Core.Common;
using Ultra.Web.Core.Interface;

namespace Ultra.Web.Core.Class
{    
    public class MockSession : IMockSession
    {
        protected int _ExpireMinute;
        protected Dictionary<string, object> dicKey;

        public MockSession()
        {
            this._ExpireMinute = 20;
            this.dicKey = new Dictionary<string, object>(30);
        }

        public MockSession(int expireMinute)
        {
            this._ExpireMinute = 20;
            this.dicKey = new Dictionary<string, object>(30);
            this._ExpireMinute = expireMinute;
        }

        protected virtual string BuildCookieKey(string key)
        {
            return string.Format("{0}_Ultra_Cookie", key);
        }

        public virtual string SeriesObjectJson(object obj)
        {
            if (obj == null)
            {
                return string.Empty;
            }
            return JsonConvert.SerializeObject(obj);
        }

        public virtual IEnumerable<string> AllKeys
        {
            get
            {
                return HttpContext.Current.Request.Cookies.AllKeys;
            }
        }

        public virtual object this[string key]
        {
            get
            {
                string str = this.BuildCookieKey(key);
                if (!HttpContext.Current.Request.Cookies.AllKeys.Contains<string>(str))
                {
                    return null;
                }
                DateTime time = DateTime.Parse(HttpContext.Current.Request.Cookies[str].Value);
                if (DateTime.Now.DateDiff(EnDatePart.MINUTE, time) > this._ExpireMinute)
                {
                    return null;
                }
                string str2 = HttpContext.Current.Request.Cookies[key].Value;
                if (string.IsNullOrEmpty(str2))
                {
                    return null;
                }
                HttpContext.Current.Request.Cookies[key].Expires = DateTime.Now.AddMinutes((double) this._ExpireMinute);
                return JsonConvert.DeserializeObject(str2);
            }
            set
            {
                string str = this.BuildCookieKey(key);
                if (!this.dicKey.ContainsKey(str))
                {
                    this.dicKey.Add(str, value);
                }
                else
                {
                    this.dicKey[str] = value;
                }
                HttpContext.Current.Response.Cookies[str].Value = DateTime.Now.ToDefaultStr();
                HttpContext.Current.Response.Cookies[key].Value = this.SeriesObjectJson(value);
                DateTime time = DateTime.Now.AddMinutes((double) this._ExpireMinute);
                HttpContext.Current.Response.Cookies[str].Expires = time;
                HttpContext.Current.Response.Cookies[key].Expires = time;
            }
        }
    }
}

