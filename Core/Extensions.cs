using System.IO;

namespace NetDotNet.Core
{
    internal static class Extensions
    {
        internal static ulong Length(this StreamReader str)
        {
            ulong len = 0;
            while (! str.EndOfStream)
            {
                str.Read();
                len++;
            }
            return len;
        }
    }
}
