using NetDotNet.API.Cookies;
using NetDotNet.Core.IO.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDotNet.Core.Managers
{
    internal static class CookieManager
    {
        private static List<Cookie> cookies = new List<Cookie>();

        internal static void DeleteCookie(Cookie c)
        {
            lock (cookies)
            {
                cookies.Remove(c);
            }
        }

        internal static void AddCookie(Cookie c)
        {
            lock (cookies)
            {
                cookies.Add(c);
            }
        }

        internal static List<Cookie> GetCookiesForPage(string uri)
        {
            return (from c in cookies
                    where c.Path == uri
                       || c.Path == "/"
                    select c).ToList();
        }
    }
}
