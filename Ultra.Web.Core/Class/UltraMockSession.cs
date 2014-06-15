using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Ultra.Web.Core.Common;

namespace Ultra.Web.Core.Class
{
    public class UltraMockSession<T> : MockSession
    {
        protected static UltraMockSession<T> _Index;

        static UltraMockSession()
        {
            UltraMockSession<T>._Index = new UltraMockSession<T>();
        }

        public UltraMockSession()
        {
        }

        public UltraMockSession(int expireMinute) : base(expireMinute)
        {
        }

        public override IEnumerable<string> AllKeys
        {
            get
            {
                return base.AllKeys;
            }
        }

        public static UltraMockSession<T> Index
        {
            get
            {
                return UltraMockSession<T>._Index;
            }
        }

        public T this[string key]
        {
            get
            {
                string str = this.BuildCookieKey(key);
                if (!HttpContext.Current.Request.Cookies.AllKeys.Contains<string>(str))
                {
                    return default(T);
                }
                DateTime time = DateTime.Parse(HttpContext.Current.Request.Cookies[str].Value);
                if (DateTime.Now.DateDiff(EnDatePart.MINUTE, time) > base._ExpireMinute)
                {
                    return default(T);
                }
                string str2 = HttpContext.Current.Request.Cookies[key].Value;
                if (string.IsNullOrEmpty(str2))
                {
                    return default(T);
                }
                HttpContext.Current.Request.Cookies[key].Expires = DateTime.Now.AddMinutes((double) base._ExpireMinute);
                return JsonConvert.DeserializeObject<T>(str2);
            }
            set
            {
                string str = this.BuildCookieKey(key);
                if (!base.dicKey.ContainsKey(str))
                {
                    base.dicKey.Add(str, value);
                }
                else
                {
                    base.dicKey[str] = value;
                }
                HttpContext.Current.Response.Cookies[str].Value = DateTime.Now.ToDefaultStr();
                HttpContext.Current.Response.Cookies[key].Value = this.SeriesObjectJson(value);
                DateTime time = DateTime.Now.AddMinutes((double) base._ExpireMinute);
                HttpContext.Current.Response.Cookies[str].Expires = time;
                HttpContext.Current.Response.Cookies[key].Expires = time;
            }
        }
    }
}

