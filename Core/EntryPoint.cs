using System;
using NetDotNet.SocketLayer;
using System.Net.Http;
using NetDotNet.Core.IO;
using NetDotNet.API.Requests;
using NetDotNet.API.Results;
using System.Collections.Generic;


namespace NetDotNet.Core
{
    class EntryPoint
    {
        private static Dictionary<string, Page> pages = new Dictionary<string, Page>();
        private static Dictionary<string, Page> resources = new Dictionary<string, Page>();
        private static Dictionary<HTTPCode, Page> specials = new Dictionary<HTTPCode, Page>();

        public static void Main(string[] args)
        {
            foreach (HTTPCode code in HTTPCode.List())
            {
                Console.WriteLine(code.SpecialLocation);
            }
            Console.ReadLine();
        }

        private static void LoadPages()
        {

        }

        private static void LoadResources()
        {

        }

        private static void LoadSpecials()
        {
            foreach (HTTPCode code in HTTPCode.List())
            {

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
