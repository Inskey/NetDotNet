﻿using System;
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
            foreach (string f in Directory.GetFiles(path))
            {
                files.Add(f.Replace('\\', '/'));
            }
            foreach (string f in Directory.GetDirectories(path))
            {
                files.AddRange(ListAllFiles(f));
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

        internal static StreamReader ByteStream(byte[] data)
        {
            MemoryStream mstr = new MemoryStream();
            StreamWriter wtr = new StreamWriter(mstr);
            wtr.Write(data);
            wtr.Flush();
            mstr.Position = 0;
            return new StreamReader(mstr);
        }

        private static Random r = new Random();
        internal static string RandomString()
        {
            string s = "";

            for (short i = 0; i < 32; i++)
            {
                int t = r.Next(3);
                if (t == 0)
                {
                    s += (char) (r.Next(10) + 48); // ASCII 0-9
                }
                else if (t == 1)
                {
                    s += (char) (r.Next(24) + 65); // ASCII A-Z
                }
                else
                {
                    s += (char) (r.Next(24) + 97); // ASCII a-z (Yes, URLs are sometimes considered case-insensitive, but they should be case-sensitive)
                }
            }

            return s;
        }
    }
}
