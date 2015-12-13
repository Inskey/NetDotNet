using NetDotNet.Core;
using System.IO;
using System.Text;


namespace NetDotNet.API.Results
{
    public class CustomBody : ResultBody
    {
        private byte[] raw;
        private Stream stream;
        private bool inMemory;

        public CustomBody(string raw)
        {
            this.raw = Encoding.UTF8.GetBytes(raw);
            inMemory = true;
        }
        public CustomBody(Stream s)
        {
            stream = s;
            inMemory = false;
        }
        public CustomBody(byte[] raw)
        {
            this.raw = raw;
            inMemory = true;
        }

        public bool KeptInMemory()
        {
            return inMemory;
        }

        public byte[] ToRaw()
        {
            return raw;
        }

        public StreamReader GetStream()
        {
            return new StreamReader(stream);
        }

        public ulong GetLength()
        {
            if (inMemory)
            {
                return (ulong) raw.Length;
            }
            else
            {
                return GetStream().Length();
            }
        }

        public void Unload()
        {
            if (stream != null)
            {
                stream.Close();
            }
        }
    }
}
