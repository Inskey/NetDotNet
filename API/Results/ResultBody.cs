using System.IO;


namespace NetDotNet.API.Results
{
    public interface ResultBody
    {
        bool UseStream();
        string ToRaw();
        StreamReader GetStream();
    }
}
