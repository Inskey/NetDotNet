using System;


namespace NetDotNet.Core
{
    internal static class Util
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
    }
}
