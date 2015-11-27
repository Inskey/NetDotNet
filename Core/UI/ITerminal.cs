using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDotNet.Core.UI
{
    // Provide a common interface for Linux and Windows terminal classes
    internal interface ITerminal
    {
        void Init();
        void Deinit();
        void WriteLine(string line);
        void Write(string str);
        string ReadLine();
    }
}
