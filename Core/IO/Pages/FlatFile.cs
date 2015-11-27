using NetDotNet.API.Requests;
using NetDotNet.API.Results;


namespace NetDotNet.Core.IO.Pages
{
    internal class FlatFile : IPage
    {
        private File file;

        internal FlatFile(string path)
        {
            file = new File(path, ServerProperties.StreamFile(path));
        }

        Result IPage.Get(Request request)
        {
            Result r = new Result();



            r.Body = file;

            return r;
        }
    }
}
