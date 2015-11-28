using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace NetDotNet.Core.UI
{
    // Since on a Windows terminal, writes will override what is being typed, this special class works around that.
    class WinTerminal : ITerminal
    {
        private bool alive;

        private List<string> display_lines = new List<string>();

        private string read_buffer = "";
        private ManualResetEventSlim readline_event = new ManualResetEventSlim(false);


        void ITerminal.Init()
        {
            alive = true;
            new Thread(ReadLoop).Start();
        }

        void ITerminal.Deinit()
        {
            alive = false;
        }

        private object update_lock = new object();
        private void Update()
        {
            lock (update_lock)
            {
                Console.Clear();

                int display_area = Console.WindowHeight - 2;

                int dif = display_area - display_lines.Count;
                if (dif > 0)
                {
                    for (byte i = 0; i < dif; i++) Console.WriteLine();
                }

                foreach (string line in display_lines) Console.WriteLine(line);

                Console.WriteLine();
                Console.Write(">" + read_buffer);
            }

        }

        private void ReadLoop()
        {
            while (alive)
            {
                var k = Console.ReadKey(true);

                if (k.Key == ConsoleKey.Enter)
                {
                    readline_event.Set();
                    Thread.Sleep(10);
                    readline_event.Reset();
                    (this as ITerminal).WriteLine(read_buffer);
                    read_buffer = "";
                }
                else if (k.Key == ConsoleKey.Backspace )
                {
                    if (read_buffer.Length == 0)
                    {
                        Console.Beep();
                    }
                    else
                    {
                        read_buffer = read_buffer.Remove(read_buffer.Length - 1);
                    }
                }
                else if (k.Key == ConsoleKey.Escape)
                {
                    read_buffer = "";
                }
                else
                {
                    read_buffer += k.KeyChar;
                }

                Update();

            }
        }

        private bool prev_was_write = false;
        void ITerminal.WriteLine(string line)
        {
            string[] split = line.Split('\n');
            lock (update_lock)
            {
                byte i;
                if (prev_was_write)
                {
                    display_lines[display_lines.Count - 1] += split[0];
                    i = 1;
                }
                else
                {
                    i = 0;
                }

                if (display_lines.Count >= 100)
                {
                    for (int j = i; j < split.Length; j++) display_lines.RemoveAt(0);
                }

                for (; i < split.Count(); i++) display_lines.Add(split[i]);
            }

            prev_was_write = false;
            Update();
        }

        void ITerminal.Write(string str)
        {
            string[] split = str.Split('\n');
            lock (update_lock)
            {
                if (prev_was_write)
                {
                    display_lines[display_lines.Count - 1] += split[0];
                    for (byte i = 1; i < split.Length; i++)
                    {
                        display_lines.RemoveAt(0);
                        display_lines.Add(split[i]);
                    }
                }
                else
                {
                    if (display_lines.Count >= 100)
                    {
                        for (byte i = 0; i < split.Length; i++) display_lines.RemoveAt(0);
                    }

                    foreach (string l in split) display_lines.Add(l);
                }
            }

            prev_was_write = true;
            Update();
        }

        string ITerminal.ReadLine()
        {
            readline_event.Wait();
            return read_buffer;
        }

    }
}
