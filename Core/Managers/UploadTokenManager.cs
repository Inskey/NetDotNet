using NetDotNet.API.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDotNet.Core.Managers
{
    internal static class UploadTokenManager
    {
        private static List<UploadToken> uploadTokens = new List<UploadToken>();

        internal static void RemoveUT(UploadToken t)
        {
            lock (uploadTokens)
            {
                uploadTokens.Remove(t);
            }
        }

        internal static void AddUT(UploadToken t)
        {
            lock (uploadTokens)
            {
                uploadTokens.Add(t);
            }
        }

        internal static List<UploadToken> GetUTs()
        {
            return new List<UploadToken>(uploadTokens);
        }
    }
}
