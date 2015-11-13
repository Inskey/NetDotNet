using NetDotNet.API;
using NetDotNet.API.Requests;
using NetDotNet.API.Results;


namespace NetDotNet.Core.IO.Pages
{
    internal class DynamicGenerator : Page
    {
        private PageGenerator generator;

        internal DynamicGenerator(PageGenerator gen)
        {
            generator = gen;
        }

        Result Page.Get(Request request)
        {
            return generator.Get(request);
        }
    }
}
