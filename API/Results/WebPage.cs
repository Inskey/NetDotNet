using NetDotNet.API.HTMLComponents;
using System.IO;
using System.Text;

namespace NetDotNet.API.Results
{
    public class WebPage : ResultBody
    {
        public Head head;
        public Body body;

        public bool KeptInMemory()
        {
            return true;
        }

        public byte[] ToRaw()
        {
            return Encoding.UTF8.GetBytes(head.ToRaw() + body.ToRaw());
        }

        public StreamReader GetStream()
        {
            MemoryStream s = new MemoryStream();
            StreamWriter r = new StreamWriter(s);
            byte[] b = ToRaw();
            r.Write(b);
            s.Flush();
            s.Position = 0;
            return new StreamReader(s);
        }

        public ulong GetLength()
        {
            return 0;
        }

        public void Unload()
        {

        }
    }
}
