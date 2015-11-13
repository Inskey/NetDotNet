using System;
using System.Collections.Generic;
using System.IO;

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

        internal static List<string> ListAllFiles(string path)
        {
            var files = new List<string>();
            files.AddRange(Directory.GetFiles(path));
            foreach (string f in Directory.GetDirectories(path))
            {
                files.AddRange(ListAllFiles(path));
            }
            return files;
        }

        internal static StreamReader StringStream(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return new StreamReader(stream);
        }
    }
}
