using System;
using NetDotNet.Core.UI;


namespace NetDotNet.Core
{
    internal static class Logger
    {
        private static ITerminal term;
        internal static void SetTerminal(ITerminal term)
        {
            Logger.term = term;
        }

        private static object lck = new object();
        internal static void Log(LogLevel level, string[] lines)
        {
            if (lines.Length == 0) return;
            lock (lck)
            {
                term.WriteLine(DateTime.Now.ToString("[HH:mm:ss] ") + (level == LogLevel.Normal ? "STD" : (level == LogLevel.Error ? "ERR" : "SEV")) + ": " + lines[0]);
                for (int i = 1; i < lines.Length; i++)
                {
                    term.WriteLine(lines[i]);
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
