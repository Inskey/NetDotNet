﻿using NetDotNet.Core;
using NetDotNet.Core.Expiration;
using NetDotNet.Core.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDotNet.API.Security
{
    public class DownloadToken : IExpirable
    {
        /// <summary>
        /// When should this upload token expire? Leave null for no expiration (not recommended)
        /// </summary>
        public DateTime? Expiration { get; set; }
        /// <summary>
        /// The actual location of the file
        /// </summary>
        public string RealPath { get; set; }
        /// <summary>
        /// The unique "path" the client will access, which doesn't really exist. Will be generated by the constructor.
        /// </summary>
        public readonly string PublicURI;
        /// <summary>
        /// IP of the client who is allowed to upload (eg. 127.0.0.1)
        /// </summary>
        public string ClientIP { get; set; }

        public DownloadToken()
        {
            PublicURI = "/dtokens/" + Util.RandomString();
        }

        public DownloadToken(string realPath)
        {
            RealPath = realPath;
            PublicURI = "/dtokens/" + Util.RandomString();
        }

        /// <summary>
        /// If you want the token to expire before the set time, call this method. 
        /// </summary>
        public void Expire(bool early)
        {
            if (early)
            {
                Expirer.Expire(this);
            }
            DownloadTokenManager.RemoveDT(this);
        }

        DateTime? IExpirable.GetExpiration()
        {
            return Expiration;
        }
    }
}
