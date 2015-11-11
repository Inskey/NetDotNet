using NetDotNet.API.HTMLComponents.Attributes;
using System.Collections.Generic;


namespace NetDotNet.API.HTMLComponents
{
    public class Link : NonterminatingComponent
    {        
        public Link(LinkAttributes attr, params HTMLComponent[] subComponents)
        {
            this.subComponents = new List<HTMLComponent>(subComponents);
            attributes = attr;
        }

        public override string ToRaw()
        {
            string raw = "<a";

            return raw;
        }
    }
}
