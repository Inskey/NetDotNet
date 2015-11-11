using NetDotNet.API.Results;
using NetDotNet.API.Requests;


namespace NetDotNet.Core.IO
{
    internal interface Page
    {
        Result Get(Request request);
    }
}
