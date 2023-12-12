using System.Threading;
using System;

namespace Common.Core.Threading
{
    public class SimpleMonitor : IDisposable
    {
        private int _busyCount;

        public void Dispose()
        {
            Interlocked.Decrement(ref _busyCount);
        }

        public void Enter()
        {
            Interlocked.Increment(ref _busyCount);
        }

        public bool Busy => Volatile.Read(ref _busyCount) > 0;
    }
}