using NetDotNet.API.Results;
using NetDotNet.API;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;

namespace NetDotNet.Core.IO.Pages
{
    internal static class Loader
    {
        internal static DynamicGenerator LoadGenerator(string path)
        {
            string absolutepath = Directory.GetCurrentDirectory().TrimEnd(Path.PathSeparator) + Path.PathSeparator + path;
            Type[] types = null;
            try
            {
                types = Assembly.LoadFile(absolutepath).GetTypes();
            }
            catch (FileLoadException e)
            {
                Logger.Log(LogLevel.Error, new[] {
                    "Failed to load assembly at " + path + " due to FileLoadException. Further details:",
                    "Message: " + e.Message,
                    "Stacktrace: " + e.StackTrace
                });
                return null;
            }
            catch (FileNotFoundException e)
            {
                Logger.Log(LogLevel.Error, new[] {
                    "Failed to load assembly at " + path + " due to FileNotFoundException. (This should in theory never happen unless a file was deleted at exactly the right moment.) Further details:",
                    "Message: " + e.Message,
                    "Stacktrace: " + e.StackTrace
                });
                return null;
            }
            catch (BadImageFormatException e)
            {
                Logger.Log(LogLevel.Error, new[] {
                    "Failed to load assembly at " + path + " due to BadImageFormatException. Further details:",
                    "Message: " + e.Message,
                    "Stacktrace: " + e.StackTrace
                });
                return null;
            }
            var classes = types.Where(a => a.IsClass).ToList();
            if (classes.Count() == 0)
            {
                Logger.Log(LogLevel.Error, "The assembly at " + path + " did not contain any classes, and will not be loaded.");
                return null;
            }
            classes = classes.Where(a => a.GetInterfaces().Contains(typeof(PageGenerator))).ToList();
            if (classes.Count() == 0)
            {
                Logger.Log(LogLevel.Error, "The assembly at " + path + " did not contain any classes implementing the NetDotNet.API.PageGenerator interface. It will not be loaded.");
                return null;
            }

            return (DynamicGenerator) Activator.CreateInstance(classes[0]);
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
                // get from /Specials
            }
        }
    }
}
