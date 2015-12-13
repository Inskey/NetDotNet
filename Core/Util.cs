using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NetDotNet.Core
{
    internal static class Util
    {
        internal static bool IsLinux()
        {
            int p = (int) Environment.OSVersion.Platform;
            return (p == 4) || (p == 6) || (p == 128);
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
            writer.Write(Encoding.UTF8.GetBytes(s));
            writer.Flush();
            stream.Position = 0;
            return new StreamReader(stream);
        }
    }
}
