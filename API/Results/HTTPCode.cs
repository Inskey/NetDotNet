using System.Collections.Generic;
using System.Linq;


namespace NetDotNet.API.Results
{
    public class HTTPCode
    {
        public short ID { get; private set; }
        public string Message { get; private set; }
        internal string SpecialLocation { get; private set; }

        internal static List<HTTPCode> ListSpecials()
        {
            return (from pp in 
                        (from p in typeof(HTTPCode).GetFields()
                        where p.FieldType == typeof(HTTPCode)
                        select p.GetValue(p) as HTTPCode)
                    where pp.SpecialLocation != null
                    select pp).ToList();
        }

        public static HTTPCode Success = new HTTPCode {
            ID      = 200,
            Message = "OK" };
        public static HTTPCode Unauthorized = new HTTPCode {
            ID      = 401,
            Message = "Authorization Required",
            SpecialLocation = "401_Unauthorized" };
        public static HTTPCode NotFound = new HTTPCode {
            ID      = 404,
            Message = "Unable To Find Resource",
            SpecialLocation = "404_NotFound" };
    }
}
