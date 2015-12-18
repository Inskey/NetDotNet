using System.Collections.Generic;


namespace NetDotNet.Core
{
    internal static class ServerProperties
    {
        internal const string Version = "NetDotNet/1.0.0";
        internal static ushort MaxHeaderLength;
        internal static ulong MaxPostLength;
        internal static bool UseTarpit;
        internal static byte MaxConnsPerIP;
        internal static long RequestTimeout;
        internal static ushort BytesPerPckt;
        internal static int PcktDelay;
        internal static int TCPBacklog;
        internal static string Accept_Ranges;

        internal static string Domain; // for example, http://example.com/

        private static List<string> filesToStream = new List<string>(); // Files that should be sent using a FileStream rather than kept in memory, recommended for larger files
        internal static bool StreamFile(string path)
        {
            return filesToStream.Contains(path);
        }
    }
}
