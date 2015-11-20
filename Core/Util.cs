using System;
using System.Collections.Generic;
using System.IO;

namespace NetDotNet.Core
{
    internal static class Util
    {
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
