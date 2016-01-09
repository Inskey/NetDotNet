using NetDotNet.API.Security;
using NetDotNet.Core;
using NetDotNet.Core.Managers;
using System.Collections.Generic;
using System.Linq;


namespace NetDotNet.API
{
    // !!! work on this once ServerProperties is complete !!!
    public static class Server
    {
        // Options--descriptions of each can be found in the server.properties file. I've used different convention to save space here since these are so repetitive.
        public static string Version() {
            return ServerProperties.Version;
        }
        public static string Domain() {
            return ServerProperties.Domain;
        }
        public static byte MaxConnectionsPerIP() {
            return ServerProperties.MaxConnsPerIP;
        }
        public static ushort MaxHeaderLength() {
            return ServerProperties.MaxHeaderLength;
        }
        public static ulong MaxPostLength() {
            return ServerProperties.MaxPostLength;
        }
        public static int DelayBetweenPackets() {
            return ServerProperties.PcktDelay;
        }

        public static bool UseHTTPTarpit() { 
            return ServerProperties.UseTarpit;
        }
        public static string Accept_Ranges() {
            return ServerProperties.Accept_Ranges;
        }
        public static ushort BytesPerTCPPacket() {
            return ServerProperties.BytesPerPckt;
        }

        // Token stuff
        public static List<UploadToken> GetUploadTokensForIP(string addr)
        {
            return (from ut in UploadTokenManager.GetUTs()
                    where ut.ClientIP == addr
                    select ut)
                   .ToList();
        }
    }
}
