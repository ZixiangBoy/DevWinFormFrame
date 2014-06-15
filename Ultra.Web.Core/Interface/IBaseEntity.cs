using System;

namespace Ultra.Web.Core.Interface
{
    public interface IBaseEntity
    {
        System.Guid Guid { get; set; }

        int Id { get; set; }

        bool UISelected { get; set; }
    }
}

