using System;
using System.Runtime.CompilerServices;

namespace Ultra.Web.Core.Common
{
    [Serializable]
    public class DateTimeRange
    {
        public override string ToString()
        {
            return (this.Begin.ToDefaultStr() + this.End.ToDefaultStr().ToLower());
        }

        public DateTime Begin { get; set; }

        public DateTime End { get; set; }

        public int UserDef { get; set; }

        public bool UserMark { get; set; }
    }
}

