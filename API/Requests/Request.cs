using System.Collections.Generic;


namespace NetDotNet.API.Requests
{
    public class Request
    {
        internal Request(string raw)
        {
            string line;
            while ((line = raw.Substring(0, raw.IndexOf("\r\n"))) != null)
            {

            }
        }

        public RequestType Type;

        public string URI;
        public string Version;

        public string User_Agent;
        public string Host;
        public string Content_Type;
        public short Content_Length;
        public List<string> Accept_Language;
        public List<string> Accept_Encoding;
        public List<string> Accept_Charset;
        public bool Keep_Alive;
    }
}
