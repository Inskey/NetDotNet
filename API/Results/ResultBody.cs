using System.IO;


namespace NetDotNet.API.Results
{
    public interface ResultBody
    {
        bool KeptInMemory();
        byte[] ToRaw();
        StreamReader GetStream();
        ulong GetLength();
        void Unload();
    }
}
