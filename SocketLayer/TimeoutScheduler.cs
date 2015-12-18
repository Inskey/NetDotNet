using System.Threading;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Collections.Concurrent;
using NetDotNet.Core;
using NetDotNet.Core.Expiration;

namespace NetDotNet.SocketLayer
{
    internal static class TimeoutScheduler
    {
        private static ConcurrentDictionary<long, IExpirable> timeouts = new ConcurrentDictionary<long, IExpirable>();
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
                foreach (KeyValuePair<long, IExpirable> p in timeouts
                    .Where(l => l.Key > ms))
                {
                    p.Value.Expire();
                }
                    
                Thread.Sleep(100);
            }
        }

        internal static void AddTimeout(HTTPConnection conn)
        {
            timeouts.TryAdd(GetMS() + ServerProperties.RequestTimeout, conn);
        }

        internal static void Kill()
        {
            alive = false;
        }

        private static long GetMS()
        {
            return  DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }
    }
}
