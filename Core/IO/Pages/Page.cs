using NetDotNet.API.Results;
using NetDotNet.API.Requests;


namespace NetDotNet.Core.IO.Pages
{
    internal interface Page
    {
        Result Get(Request request);
    }
}
