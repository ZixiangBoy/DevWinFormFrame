using System;
using System.Collections.Generic;
using System.Reflection;

namespace Ultra.Web.Core.Interface
{   
    public interface IMockSession
    {
        string SeriesObjectJson(object obj);

        IEnumerable<string> AllKeys { get; }

        object this[string key] { get; set; }
    }
}

