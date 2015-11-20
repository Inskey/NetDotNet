using NetDotNet.API.Requests;
using NetDotNet.API.Results;


namespace NetDotNet.Core.IO.Pages
{
    internal class FlatFile : Page
    {
        private File file;

        internal FlatFile(string path)
        {
            file = new File(path, ServerProperties.StreamFile(path));
        }

        Result Page.Get(Request request)
        {
            Result r = new Result();



            r.Body = file;

            return r;
        }
    }
}
