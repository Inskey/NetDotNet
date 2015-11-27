using NetDotNet.API.HTMLComponents;
using System.IO;

namespace NetDotNet.API.Results
{
    public class WebPage : ResultBody
    {
        public Head head;
        public Body body;

        public bool UseStream()
        {
            return false;
        }

        public string ToRaw()
        {
            return head.ToRaw() + body.ToRaw();
        }

        public StreamReader GetStream()
        {
            return null;
        }

        public short GetLength()
        {
            return 0;
        }
    }
}
