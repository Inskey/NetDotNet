using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDotNet.API.Cookies
{
    public class Cookie
    {
        /// <summary>
        /// When should the cookie expire? Leave null for a session cookie (deleted on browser closing).
        /// </summary>
        DateTime? Expiration;

        /// <summary>
        /// Only allow this cookie to be modified/transmitted over HTTP (not by JavaScript, thus less vulnerable to XSS). Default false.
        /// </summary>
        bool HTTPOnly = false;

        /// <summary>
        /// Specify the domain name (or IP) for the cookie. By default, the cookie will only be sent back to the same page that set it.
        /// </summary>
        string Domain;

        /// <summary>
        /// Specify the path on this server that the cookie should be sent back to.
        /// </summary>
        string Path;

        /// <summary>
        /// Key, or name, for the cookie.
        /// </summary>
        string Key;

        /// <summary>
        /// Value of the cookie.
        /// </summary>
        string Value;

        /// <summary>
        /// Get the cookie as it will be formatted in an HTTP response
        /// </summary>
        string ToRaw()
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
    }
}
