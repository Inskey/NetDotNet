using NetDotNet.Core;
using System.IO;


namespace NetDotNet.API.Results
{
    public class File : ResultBody
    {
        private string path;
        private bool loadInMemory;
        private byte[] content;
        private FileStream stream;
        private ulong length;

        public File(string path, bool loadInMemory = false)
        {
            this.path = path;
            this.loadInMemory = loadInMemory;

            if (loadInMemory)
            {
                content = System.IO.File.ReadAllBytes(path);
                length = (ulong) content.LongLength;
            }
            else
            {
                stream = System.IO.File.Open(path, FileMode.Open, FileAccess.ReadWrite); // Make sure another program does not attempt to write 
                length = GetStream().Length();
            }
        }

        public bool KeptInMemory()
        {
            return loadInMemory;
        }

        public byte[] ToRaw() 
        {
            return content;
        }

        public StreamReader GetStream()
        {
            return new StreamReader(stream);
        }

        public ulong GetLength()
        {
            return length;
        }

        public void Unload()
        {
            stream.Close();
        }
    }
}
