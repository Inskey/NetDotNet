using System.Threading;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Collections.Concurrent;

namespace NetDotNet.SocketLayer
{
    internal static class TimeoutScheduler
    {
        private static ConcurrentDictionary<long, HTTPConnection> timeouts = new ConcurrentDictionary<long, HTTPConnection>();
        private static bool alive = true;

        internal static void Init()
        {
            new Thread(Loop).Start();
        }

        private static void Loop()
        {
            while (alive)
            {
                long ms = GetMS();
                foreach (KeyValuePair<long, HTTPConnection> p in timeouts
                    .Where(l => l.Key > ms))
                {
                    p.Value.Close();
                }
                    
                Thread.Sleep(100);
            }
        }

        internal static void AddTimeout(HTTPConnection conn)
        {
            timeouts.TryAdd(GetMS() + 2000, conn);
        }

        internal static void Kill()
        {
            alive = false;
        }

        private static long GetMS()
        {
            return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }
    }
}
