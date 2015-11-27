namespace NetDotNet.API.Resources
{
    public abstract class Resource
    {
        internal Resource(string path)
        {
            Path = path;
            // set other shit
        }

        public string Path; // "C:\NetDotNetServer\resources\example.mp4"
        public string URI;  // "/example.mp4"
        public string URL;  // "http://example.com/example.mp4"

        public abstract HTMLComponents.IHTMLComponent Get(Requests.Request request = null);
    }
}
