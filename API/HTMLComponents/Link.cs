using System.Collections.Generic;


namespace NetDotNet.API.HTMLComponents
{
    public class Link : HTMLComponent
    {
        List<HTMLComponent> subComponents;

        public Link(params HTMLComponent[] subComponents)
        {
            this.subComponents = new List<HTMLComponent>(subComponents);
        }

        public string ToRaw()
        {
            string raw = "<a";

            return raw;
        }
    }
}
