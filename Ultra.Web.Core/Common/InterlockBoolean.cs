using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace Ultra.Web.Core.Common
{
    public class InterlockBoolean
    {
        internal long BooleanValue;

        private InterlockBoolean()
        {
        }

        public static InterlockBoolean Create(bool b = false)
        {
            return new InterlockBoolean { BooleanValue = !b ? ((long) 0) : ((long) 1) };
        }

        protected virtual bool Set(bool b)
        {
            return (Interlocked.Exchange(ref this.BooleanValue, b ? ((long) 1) : ((long) 0)) != 0L);
        }

        public virtual bool Value
        {
            get
            {
                return (Interlocked.Read(ref this.BooleanValue) != 0L);
            }
            set
            {
                this.Set(value);
            }
        }
    }
}

