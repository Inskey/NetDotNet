using System;
using System.IO;


namespace NetDotNet.API.Results
{
    public class Result
    {
        public HTTPCode Code = HTTPCode.Success;

        public DateTime Timestamp = DateTime.Now;
        // connection open/close?

        public readonly string Server = Core.ServerProperties.Version;
        // accept-ranges?

        public string[] Content_Type = { "text/html" };
        public DateTime Last_Modified; // how do we want to keep up with this?

        public ResultBody Body;

        public Result()
        {

        }

        internal string GetHeader()
        {
            return null;
            // make header here
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
