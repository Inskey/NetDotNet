﻿using NetDotNet.API.Requests;
using NetDotNet.API.Results;


namespace NetDotNet.Core.IO
{
    internal class FlatFile : Page
    {
        private File file;

        internal FlatFile(string path, bool stream)
        {
            file = new File(path);
        }

        Result Page.Get(Request request)
        {
            Result r = new Result();
            r.Body = file;
            return null;
        }
    }
}