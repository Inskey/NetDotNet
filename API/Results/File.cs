using System.IO;
using System.Text;


namespace NetDotNet.API.Results
{
    public class File : ResultBody
    {
        private string path;
        private bool useStream;
        private string content;

        public File(string path, bool useStream)
        {
            this.path = path;
            this.useStream = useStream;

            if (! useStream)
            {
                content = Encoding.UTF8.GetString(System.IO.File.ReadAllBytes(path));
            }
        }

        public bool UseStream()
        {
            return useStream;
        }

        public string ToRaw() 
        {
            return content;
        }

        public StreamReader GetStream()
        {
            return new StreamReader(System.IO.File.OpenRead(path));
        }

        public short GetLength()
        {
            return 0;
        }
    }
}
