using NetDotNet.API.Requests;
using NetDotNet.API.Results;


namespace NetDotNet.API
{
    public interface PageGenerator
    {
        Result Get(Request request);
    }
}
