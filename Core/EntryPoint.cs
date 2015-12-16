using System;
using NetDotNet.SocketLayer;
using NetDotNet.Core.IO.Pages;
using NetDotNet.API.Requests;
using NetDotNet.API.Results;
using NetDotNet.Core.UI;
using System.Collections.Generic;
using System.IO;


namespace NetDotNet.Core
{
    // Entry point for application and "middle-man" class for interaction between socket layer and API
    class EntryPoint
    {
        private static ITerminal term;

        private static Listener listener;

        private static Dictionary<string, IPage> pages = new Dictionary<string, IPage>();
        private static Dictionary<string, IPage> resources = new Dictionary<string, IPage>();
        private static Dictionary<HTTPCode, IPage> specials = new Dictionary<HTTPCode, IPage>();


        public static void Main(string[] args)
        {
            // Set up logger depending on OS
            term = (Util.IsLinux() ? (ITerminal) new LinTerminal() : new WinTerminal());
            term.Init();

            term.WriteLine("  _   _      _     _   _  _____ _____ ");
            term.WriteLine(" | \\ | |    | |   | \\ | ||  ___|_   _|");
            term.WriteLine(" |  \\| | ___| |_  |  \\| || |__   | |  ");
            term.WriteLine(" | . ` |/ _ \\ __| | . ` ||  __|  | |  ");
            term.WriteLine(" | |\\  |  __/ |_ _| |\\  || |___  | | ");
            term.WriteLine(" \\_| \\_/\\___|\\__(_)_| \\_/\\____/  \\_/");
            term.WriteLine("---------------------------------------");
            term.WriteLine(" Revolutionize web development with C# ");
            term.WriteLine("---------------------------------------");

            Logger.SetTerminal(term);
            Logger.Log("Terminal set up for " + (Util.IsLinux() ? "Linux" : "Windows") + ".");

            // Load all the pages
            Logger.Log("Loading pages...");
            short amount = LoadPages();
            Logger.Log("Loaded " + amount + " pages.");

            foreach (var p in pages.Values)
            {
                Logger.Log(p.Get(null).GetBody().ReadToEnd());
            }

            // Load all the resources
            Logger.Log("Loading resources...");
            amount = LoadResources();
            Logger.Log("Loaded " + amount + " resources.");

            // Load special pages, or copy them to the filesystem if they don't exist
            //Logger.Log("Loading special pages...");
            //LoadSpecials();
            //Logger.Log("All specials loaded.");

            // Start listening for HTTP connections
            listener = new Listener();
            listener.Open();

            // Main thread becomes the command line thread
            ConsoleLoop();
        }

        private static short LoadPages()
        {
            if (Directory.Exists("Pages"))
            {
                short a = 0;
                foreach (var p in Util.ListAllFiles("Pages"))
                {
                    if (LoadPage(p)) a++;
                }
                return a;
            }
            else
            {
                Logger.Log("Creating /Pages directory.");
                Directory.CreateDirectory("Pages");
                return 0;
            }
        }

        private static bool LoadPage(string path)
        {
            if (path.EndsWith(".dll"))
            {
                DynamicGenerator gen = Loader.LoadGenerator(path);
                if (gen == null)
                {
                    return false;
                }
                pages.Add(path, gen);
                return true;
            }
            else
            {
                pages.Add(path, Loader.LoadFlat(path));
                return true;
            }
        }


        private static short LoadResources()
        {
            return 0;
        }

        private static void LoadSpecials()
        {
            if (! Directory.Exists("Specials"))
            {
                Directory.CreateDirectory("Specials");
            }

            foreach (HTTPCode code in HTTPCode.ListSpecials())
            {
                specials.Add(code, Loader.LoadOrCreateSpecial(code));
            }
        }

        private static void ConsoleLoop()
        {
            while (true)
            {
                string line = term.ReadLine();

                if (line == "exit")
                {
                    break;
                }
            }

            term.Deinit();
            Console.Clear();


        }

        internal static void SubscribeConnection(HTTPConnection c)
        {
            c.RequestReceived += OnRequest;
        }

        private static void OnRequest(Request r, HTTPConnection.RequestCompleteHandler callback)
        {
            IPage p;
            if (! pages.TryGetValue(r.URI, out p))
            {
                p = specials[HTTPCode.NotFound];
            }

            var res = p.Get(r);
            if (res.Keep_Alive == null)
            {
                res.Keep_Alive = r.Keep_Alive; // Unless the page specifically accepted or denied keeping the connection open, go with what the client asked.
            }

            callback(res);
        }
    }
}
