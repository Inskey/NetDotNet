namespace NetDotNet.API.HTMLComponents.Attributes
{
    public abstract class AttributesAbstract
    {
        public string Class;
        public string Style;
        public string ID;

        public virtual string GetTextRepresentation()
        {
            string tmp = "";

            if (Class != null)
            {
                tmp += "class=\"" + Class + "\" ";
            }
            if (Style != null)
            {
                tmp += "style=\"" + Style + "\" ";
            }
            if (ID != null)
            {
                tmp += "id=\"" + ID + "\" ";
            }

            return tmp;
        }
    }
}
