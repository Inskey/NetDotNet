using NetDotNet.API.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDotNet.Core.Managers
{
    internal static class DownloadTokenManager
    {
        private static List<DownloadToken> downloadTokens = new List<DownloadToken>();

        internal static void RemoveDT(DownloadToken t)
        {
            lock (downloadTokens)
            {
                downloadTokens.Remove(t);
            }
        }

        internal static void AddDT(DownloadToken t)
        {
            lock (downloadTokens)
            {
                downloadTokens.Add(t);
            }
        }

        internal static List<DownloadToken> GetDTs()
        {
            return new List<DownloadToken>(downloadTokens);
        }
    }
}
