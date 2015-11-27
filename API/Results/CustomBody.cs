using System.IO;

namespace NetDotNet.API.Results
{
    public class CustomBody : ResultBody
    {
        private string raw;
        private StreamReader stream;
        private bool useStream;

        public CustomBody(string raw)
        {
            this.raw = raw;
            useStream = false;
        }
        public CustomBody(Stream s)
        {
            stream = new StreamReader(s);
            useStream = true;
        }

        public bool UseStream()
        {
            return useStream;
        }

        public string ToRaw()
        {
            return raw;
        }

        public StreamReader GetStream()
        {
            return stream;
        }

        public short GetLength()
        {
            return 0;
        }
    }
}
