using NetDotNet.API.HTMLComponents;
using NetDotNet.API.Requests;
using System.IO;
using System.Text;


namespace NetDotNet.API.Resources
{
    public class TextResource : Resource
    {
        internal TextResource(string filePath) : base(filePath)
        {
            
        }

        public override IHTMLComponent Get(Request request)
        {
            return new TextComponent(Encoding.UTF8.GetString(File.ReadAllBytes(Path)));
        }
    }
}
