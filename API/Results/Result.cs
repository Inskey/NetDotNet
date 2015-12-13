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
        public readonly string Accept_Ranges = Core.ServerProperties.Accept_Ranges;

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
            h += "Date: " + Timestamp.ToString("R") + "\r\n"; // "R" indicates the same format HTTP requires
            h += "Connection: " + (Keep_Alive.Value ? "keep-alive" : "close") + "\r\n";
            h += "Server: " + Server + "\r\n";

            h += "\r\n"; // Blank line to separate header from body

            return h;
        }

        internal StreamReader GetBody() // HTTPConnection sees all output as a stream whether the content is really streamed or not
        {
            if (Body.KeptInMemory())
            {
                return Core.Util.StringStream(Body.ToRaw());
            }
            else
            {
                return Body.GetStream();
            }
        }
    }
}
