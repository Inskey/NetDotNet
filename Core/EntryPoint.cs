using System;
using NetDotNet.SocketLayer;
using NetDotNet.Core.IO.Pages;
using NetDotNet.API.Requests;
using NetDotNet.API.Results;
using System.Collections.Generic;
using System.IO;


namespace NetDotNet.Core
{
    // Entry point for application and "middle-man" class for interaction between socket layer and API
    class EntryPoint
    {
        private static Dictionary<string, Page> pages = new Dictionary<string, Page>();
        private static Dictionary<string, Page> resources = new Dictionary<string, Page>();
        private static Dictionary<HTTPCode, Page> specials = new Dictionary<HTTPCode, Page>();

        public static void Main(string[] args)
        {
            
        }

        private static void LoadPages()
        {
            if (Directory.Exists("pages"))
            {
                
            }
            else
            {
                Directory.CreateDirectory("pages");
            }
        }

        private static void LoadPage(string path)
        {
            if (path.EndsWith(".dll"))
            {
                try
                {
                    DynamicGenerator gen = Loader.LoadGenerator(path);
                    if (gen == null)
                    {
                        Logger.Log(LogLevel.Error, "Generator at " + path + " does not contain a valid class implementing the PageGenerator interface. It will not be loaded.");
                        return;
                    }
                    pages.Add(path, gen);

                }
                catch (Exception e)
                {
                    Logger.Log(LogLevel.Error, new[] {
                        "Encountered " + e.GetType().Name + " when loading assembly at " + path + "! It will not be loaded. Further details: ",
                        "Message: " + e.Message,
                        "Stack Trace: " + e.StackTrace
                    });
                }
            }
            else
            {
                pages.Add(path, Loader.LoadFlat(path));
            }
        }


        private static void LoadResources()
        {

        }

        private static void LoadSpecials()
        {
            if (! Directory.Exists("specials"))
            {
                Directory.CreateDirectory("specials");
            }

            foreach (HTTPCode code in HTTPCode.ListSpecials())
            {
                specials.Add(code, Loader.LoadOrCreateSpecial(code));
            }
        }

        internal static void SubscribeConnection(HTTPConnection c)
        {
            c.RequestReceived += OnRequest;
        }

        private static void OnRequest(Request r, HTTPConnection.RequestCompleteHandler callback)
        {
            Page p;
            if (! pages.TryGetValue(r.URI, out p))
            {
                p = specials[HTTPCode.NotFound];
            }

            callback(p.Get(r));
        }
    }
}
