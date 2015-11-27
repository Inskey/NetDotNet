using NetDotNet.API;
using NetDotNet.API.Requests;
using NetDotNet.API.Results;


namespace NetDotNet.Core.IO.Pages
{
    // Wrapper around PageGenerator
    internal class DynamicGenerator : IPage
    {
        private IPageGenerator generator;

        internal DynamicGenerator(IPageGenerator gen)
        {
            generator = gen;
        }

        Result IPage.Get(Request request)
        {
            return generator.Get(request);
        }
    }
}
