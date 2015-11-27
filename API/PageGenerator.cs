using NetDotNet.API.Requests;
using NetDotNet.API.Results;


namespace NetDotNet.API
{
    public interface IPageGenerator
    {
        Result Get(Request request);
        Result Post(Request request);
    }
}
