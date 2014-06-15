using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using Ultra.Web.Core.Interface;

namespace Ultra.Web.Core.Class
{
    public class UltraCookie<T> : IUltraCookie<T>
    {
        protected static UltraCookie<T> _Index;

        static UltraCookie()
        {
            UltraCookie<T>._Index = new UltraCookie<T>();
        }

        public T DeSeriesObject<T>(string jsvlu)
        {
            if (string.IsNullOrEmpty(jsvlu))
            {
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(jsvlu);
        }

        public string SeriesObjectJson(object obj)
        {
            if (obj == null)
            {
                return string.Empty;
            }
            return JsonConvert.SerializeObject(obj);
        }

        public IEnumerable<string> AllKeys
        {
            get
            {
                return HttpContext.Current.Request.Cookies.AllKeys;
            }
        }

        public static UltraCookie<T> Index
        {
            get
            {
                return UltraCookie<T>._Index;
            }
        }

        public T this[string key]
        {
            get
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies[key];
                if (cookie == null)
                {
                    return default(T);
                }
                if (string.IsNullOrEmpty(cookie.Value))
                {
                    return default(T);
                }
                return this.DeSeriesObject<T>(cookie.Value);
            }
            set
            {
                if (value == null)
                {
                    HttpContext.Current.Response.Cookies[key].Value = null;
                }
                else
                {
                    HttpContext.Current.Response.Cookies[key].Value = this.SeriesObjectJson(value);
                }
            }
        }
    }
}

