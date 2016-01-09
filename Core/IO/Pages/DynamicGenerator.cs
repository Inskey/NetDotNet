using NetDotNet.API;
using NetDotNet.API.Requests;
using NetDotNet.API.Results;


namespace NetDotNet.Core.IO.Pages
{
    // Wrapper around PageGenerator
    internal class DynamicGenerator : IPage
    {
        private IPageGenerator generator;
        private string path;

        internal DynamicGenerator(IPageGenerator gen, string path)
        {
            generator = gen;
        }

        Result IPage.Get(Request request)
        {
            return generator.Get(request);
        }

        Result IPage.Post(Request request)
        {
            return generator.Post(request);
        }

        string IPage.GetPath()
        {
            return path;
        }
    }
}
