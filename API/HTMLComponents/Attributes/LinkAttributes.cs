namespace NetDotNet.API.HTMLComponents.Attributes
{
    public class LinkAttributes : AttributesAbstract
    {
        public string HREF;

        public override string GetTextRepresentation()
        {
            string tmp = base.GetTextRepresentation();

            if (HREF != null)
            {
                tmp += "href=\"" + HREF + "\"";
            }

            return tmp;
        }
    }
}
