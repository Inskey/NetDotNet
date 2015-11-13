using NetDotNet.API.Results;
using NetDotNet.API;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System;

namespace NetDotNet.Core.IO.Pages
{
    internal static class Loader
    {
        internal static DynamicGenerator LoadGenerator(string path)
        {
            return (from t in Assembly.LoadFile(path).GetTypes()
                    where t.IsClass
                    where t.GetInterfaces().Contains(typeof(PageGenerator))
                    select new DynamicGenerator((PageGenerator) Activator.CreateInstance(t))).FirstOrDefault();
        }

        internal static FlatFile LoadFlat(string path)
        {
            return new FlatFile(path);
        }

        internal static Page LoadOrCreateSpecial(HTTPCode code)
        {
            string loc = "specials/" + code.SpecialLocation;
            if (System.IO.File.Exists(loc + ".dll"))
            {
                return LoadGenerator(loc + ".dll");
            }
            else if (System.IO.File.Exists(loc + ".html"))
            {
                return LoadFlat(loc + ".html");
            }
            else
            {
                
            }
        }
    }
}
