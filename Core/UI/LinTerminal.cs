using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDotNet.Core.UI
{
    // Major issues with this -- command buffer and such needs to be made, probably going to have to copy over some code from WinTerminal
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
