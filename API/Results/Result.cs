using System;
using System.IO;


namespace NetDotNet.API.Results
{
    public class Result
    {
        public HTTPCode Code = HTTPCode.Success;
        public ResultBody Body;
        public DateTime Timestamp = DateTime.Now;

        public Result()
        {
            
        }

        public StreamReader ToRaw()
        {
            return null;
        }
    }
}
