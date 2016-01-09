using NetDotNet.API.Results;
using NetDotNet.API.Requests;


namespace NetDotNet.Core.IO.Pages
{
    // Common point between flat files and dynamic content generators
    internal interface IPage
    {
        Result Get(Request request);
        Result Post(Request request);

        string GetPath();
    }
}
