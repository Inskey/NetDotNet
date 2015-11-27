namespace NetDotNet.API.HTMLComponents
{
    public class TextComponent : IHTMLComponent
    {
        private string text;

        public TextComponent(string text)
        {
            this.text = text;
        }

        public string ToRaw()
        {
            return text;
        }
    }
}
