using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDotNet.Core.UI
{
    // Since Mono will not cause writes to override what is being typed, this is just a wrapper around Console.
    class LinTerminal : ITerminal
    {
        void ITerminal.Init()
        {

        }

        void ITerminal.Deinit()
        {

        }

        void ITerminal.WriteLine(string line)
        {
            Console.WriteLine(line);
        }

        void ITerminal.Write(string str)
        {
            Console.Write(str);
        }

        string ITerminal.ReadLine()
        {
            return Console.ReadLine();
        }
    }
}
