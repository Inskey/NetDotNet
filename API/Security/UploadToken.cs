﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetDotNet.Core;
using NetDotNet.Core.Expiration;
using NetDotNet.Core.Managers;

namespace NetDotNet.API.Security
{
    public class UploadToken : IExpirable
    {
        /// <summary>
        /// When should this upload token expire? Leave null for no expiration (not recommended)
        /// </summary>
        public DateTime? Expiration { get; set; }
        /// <summary>
        /// Actual path where the file will be sent
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

        public UploadToken()
        {
            PublicURI = "/utokens/" + Util.RandomString();
        }

        public UploadToken(string realPath)
        {
            RealPath = realPath;
            PublicURI = Util.RandomString();
        }

        /// <summary>
        /// If you want the token to expire before the set time, call this method. 
        /// </summary>
        public void Expire()
        {
            UploadTokenManager.RemoveUT(this);
        }

        DateTime? IExpirable.GetExpiration()
        {
            return Expiration;
        }
    }
}
