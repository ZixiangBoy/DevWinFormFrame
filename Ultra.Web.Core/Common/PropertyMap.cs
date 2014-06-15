using System;
using System.Runtime.CompilerServices;

namespace Ultra.Web.Core.Common
{
    public class PropertyMap
    {
        public PropertyMap()
        {
        }

        public PropertyMap(string pfname)
        {
            this.TableFieldName = this.PropOrFieldName = pfname;
        }

        public PropertyMap(string tbField, string pfName)
        {
            this.TableFieldName = tbField;
            this.PropOrFieldName = pfName;
        }

        public string PropOrFieldName { get; set; }

        public string TableFieldName { get; set; }
    }
}

