using System;
using System.IO;


namespace NetDotNet.API.Results
{
    public class Result
    {
        public HTTPCode Code = HTTPCode.Success;

        public DateTime Timestamp = DateTime.Now;
        public bool? Keep_Alive = null;

        public readonly string Server = Core.ServerProperties.Version;
        // accept-ranges?

        public string Content_Type = "text/html";
        public DateTime Last_Modified; // how do we want to keep up with this?

        private ResultBody body;
        public ResultBody Body {
            get
            {
                return body;
            }
            set
            {
                body = value;

            }
        }

        public Result()
        {

        }

        internal string GetHeader()
        {
            string h = "HTTP/1.1 " + Code.ToString() + "\r\n"; // HTTP requires carriage returns
            h += "Date: " + Timestamp.ToString("R") + "\r\n";
            h += "Connection: " + (Keep_Alive.Value ? "keep-alive" : "close") + "\r\n";
            h += "Server: " + Server + "\r\n";

            h += "\r\n";

            return h;
        }

        internal StreamReader GetBody() // HTTPConnection sees all output as a stream whether the content is really streamed or not
        {
            if (Body.UseStream())
            {
                return Body.GetStream();
            }
            else
            {
                return Core.Util.StringStream(Body.ToRaw());
            }
        }
    }
}
