using NetDotNet.Core;
using NetDotNet.Core.Expiration;
using NetDotNet.Core.Managers;
using System;

namespace NetDotNet.API.Cookies
{
    public class Cookie : IExpirable
    {
        /// <summary>
        /// IP of the client "owning" the cookie.
        /// </summary>
        public string ClientIP;

        /// <summary>
        /// When should the cookie expire? Leave null for a session cookie (deleted on browser closing).
        /// </summary>
        public DateTime? Expiration;

        /// <summary>
        /// Only allow this cookie to be modified/transmitted over HTTP (not by JavaScript, thus less vulnerable to XSS). Default false.
        /// </summary>
        public bool HTTPOnly = false;

        /// <summary>
        /// Specify the domain name (or IP) for the cookie. By default, the cookie will only be sent back to the same page that set it.
        /// </summary>
        public string Domain;

        /// <summary>
        /// Specify the path on this server that the cookie should be sent back to. "/" will send it to all paths. `null` means the cookie is only sent back to the page which produced it.
        /// </summary>
        public string Path;

        /// <summary>
        /// Key, or name, for the cookie.
        /// </summary>
        public string Key;

        /// <summary>
        /// Value of the cookie.
        /// </summary>
        public string Value;

        /// <summary>
        /// Get the cookie as it will be formatted in an HTTP response
        /// </summary>
        public string ToRaw()
        {
            string raw = Key + "=" + Value;
            if (Domain != null)
            {
                raw += "; Domain=" + Domain;
            }
            if (Path != null)
            {
                raw += "; Path=" + Path;
            }
            if (Expiration != null)
            {
                raw += "; Expires=" + Expiration.Value.ToString("R");
            }
            if (HTTPOnly)
            {
                raw += "; HTTPOnly";
            }

            return raw;
        }

        /// <summary>
        /// If you want the server to stop keeping up with this cookie before it would naturally expire, call this method.
        /// </summary>
        public void Expire()
        {
            ((IExpirable) this).Expire(true);
        }
        void IExpirable.Expire(bool early)
        {
            CookieManager.DeleteCookie(this);
            if (early)
            {
                Expirer.Expire(this);
            }
        }

        DateTime? IExpirable.GetExpiration()
        {
            return Expiration;
        }
    }
}
