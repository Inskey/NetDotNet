namespace NetDotNet.API.HTMLComponents
{
    public class TextComponent : HTMLComponent
    {
        private string text;

        public TextComponent(string text)
        {
            this.text = text;
        }

        public override string ToRaw()
        {
            return text;
        }
    }
}
