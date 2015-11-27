using System.Collections.Generic;


namespace NetDotNet.API.HTMLComponents
{
    public class Link : IHTMLComponent
    {
        List<IHTMLComponent> subComponents;

        public Link(params IHTMLComponent[] subComponents)
        {
            this.subComponents = new List<IHTMLComponent>(subComponents);
        }

        public string ToRaw()
        {
            string raw = "<a";

            return raw;
        }
    }
}
