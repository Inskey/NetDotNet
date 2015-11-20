using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace NetDotNet.Core
{
    internal static class Logger
    {
        private static object lck = new object();
        internal static void Log(LogLevel level, string[] lines)
        {
            if (lines.Length == 0) return;
            lock (lck)
            {
                Console.WriteLine(DateTime.Now.ToString("[HH:mm:ss]") + (level == LogLevel.Normal ? "STD" : (level == LogLevel.Error ? "ERR" : "SEV")) + lines[0]);
                for (int i = 1; i < lines.Length; i++)
                {
                    Console.WriteLine(lines[i]);
                }
            }
        }

        internal static void Log(LogLevel level, string info)
        {
            Log(level, new[] { info });
        }

        internal static void Log(string[] lines)
        {
            Log(LogLevel.Normal, lines);
        }

        internal static void Log(string info)
        {
            Log(LogLevel.Normal, new[] { info });
        }
    }
}
