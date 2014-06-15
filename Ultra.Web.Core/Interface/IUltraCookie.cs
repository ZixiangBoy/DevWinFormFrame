using System;
using System.Collections.Generic;
using System.Reflection;

namespace Ultra.Web.Core.Interface
{   
    public interface IUltraCookie<T>
    {
        T DeSeriesObject<T>(string jsvlu);
        string SeriesObjectJson(object obj);

        IEnumerable<string> AllKeys { get; }

        T this[string key] { get; set; }
    }
}

