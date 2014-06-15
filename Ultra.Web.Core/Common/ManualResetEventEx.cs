using System;
using System.Threading;

namespace Ultra.Web.Core.Common
{
    [Serializable]
    public class ManualResetEventEx : IDisposable
    {
        private long current;
        private readonly ManualResetEvent done;
        private readonly int total;

        public ManualResetEventEx(int total)
        {
            this.total = total;
            this.current = total;
            this.done = new ManualResetEvent(false);
        }

        public void Dispose()
        {
            this.done.Dispose();
        }

        public void SetOne()
        {
            if (Interlocked.Decrement(ref this.current) == 0L)
            {
                this.done.Set();
            }
        }

        public void WaitAll()
        {
            this.done.WaitOne();
        }
    }
}

