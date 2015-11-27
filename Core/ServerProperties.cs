using System.Collections.Generic;


namespace NetDotNet.Core
{
    static internal class ServerProperties
    {
        internal const string Version = "NetDotNet/1.0.0";
        internal static short MaxRequestLength;
        internal static bool UseTarpit;
        internal static byte MaxConnsPerIP;
        internal static long RequestTimeout;

        internal static string Domain; // for example, http://example.com/

        private static List<string> filesToStream = new List<string>(); // Files that should be sent using a FileStream rather than kept in memory, recommended for larger files
        internal static bool StreamFile(string path)
        {
            return filesToStream.Contains(path);
        }
    }
}
