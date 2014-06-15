using System;

namespace Ultra.Web.Core.Interface
{
    public interface IOptionConfig
    {
        T Get<T>(string keyName);
        IOptionConfig Set<T>(string keyName, T value);
    }
}

