using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;


namespace NetDotNet.Core.Expiration
{
    internal static class Expirer
    {
        private static bool alive = false;
        private static List<IExpirable> things = new List<IExpirable>();

        internal static void Init()
        {
            alive = true;
            new Thread(ExpireLoop).Start();
        }

        internal static void Deinit()
        {
            alive = false;
        }

        private static void ExpireLoop()
        {
            while (alive)
            {
                var temp = new List<IExpirable>();
                lock (things)
                {
                    foreach (var t in things)
                    {
                        if (t.GetExpiration() <= DateTime.Now)
                        {
                            t.Expire(false);
                        }
                        else
                        {
                            temp.Add(t);
                        }
                    }
                    things = temp;
                }
                Thread.Sleep(1000);
            }
        }

        internal static void AddExpirable(IExpirable e)
        {
            lock (things)
            {
                things.Add(e);
            }
        }

        internal static void Expire(IExpirable e)
        {
            lock (things)
            {
                things.Remove(e);
            }
        }
    }
}
