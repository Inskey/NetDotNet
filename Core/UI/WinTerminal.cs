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
        private List<string> prev_cmds = new List<string>();
        private int index = -1;

        private string read_buffer = "";
        private int cursor = 1;
        private ManualResetEventSlim readline_event = new ManualResetEventSlim(false);

        private char[] valid_chars = {
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
            '1', '2', '3', '4', '5', '6', '7', '8', '9', '0',
            ' ', '~', '`', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '_', '-', '+', '=', '[', ']', '{', '}', '|', '\\', ';', ':', '\'', '"', '<', '>', ',', '.', '?', '/'
        };


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
                Console.CursorLeft = cursor;
            }

        }

        private void ReadLoop()
        {
            while (alive)
            {
                var k = Console.ReadKey(true);

                if (k.Key == ConsoleKey.Enter)
                {
                    if (read_buffer == "") continue;

                    (this as ITerminal).WriteLine(">>>" + read_buffer);

                    readline_event.Set();
                    Thread.Sleep(10);
                    readline_event.Reset();

                    index = -1;
                    if (prev_cmds.Count == 0 || read_buffer != prev_cmds[0]) prev_cmds.Insert(0, read_buffer);

                    cursor = 1;

                    read_buffer = "";
                }
                else if (k.Key == ConsoleKey.Backspace)
                {
                    if (cursor == 1)
                    {
                        Console.Beep();
                    }
                    else
                    {
                        cursor--;
                        read_buffer = read_buffer.Remove(cursor - 1, 1);
                    }
                }
                else if (k.Key == ConsoleKey.Delete)
                {
                    if (cursor == read_buffer.Length + 1)
                    {
                        Console.Beep();
                    }
                    else
                    {
                        read_buffer = read_buffer.Remove(cursor - 1, 1);
                    }
                }
                else if (k.Key == ConsoleKey.Escape)
                {
                    cursor = 1;
                    read_buffer = "";
                }
                else if (k.Key == ConsoleKey.UpArrow)
                {
                    if (index == prev_cmds.Count - 1)
                    {
                        Console.Beep();
                    }
                    else
                    {
                        index++;
                        read_buffer = prev_cmds[index];
                        cursor = read_buffer.Length + 1;
                    }
                }
                else if (k.Key == ConsoleKey.DownArrow)
                {
                    if (index == -1)
                    {
                        Console.Beep();
                    }
                    else if (index == 0)
                    {
                        index--;
                        cursor = 1;
                        read_buffer = "";
                    }
                    else
                    {
                        index--;
                        read_buffer = prev_cmds[index];
                        cursor = read_buffer.Length + 1;
                    }
                }
                else if (k.Key == ConsoleKey.LeftArrow)
                {
                    if (Console.CursorLeft > 1)
                    {
                        cursor--;
                    }
                    else
                    {
                        Console.Beep();
                    }
                }
                else if (k.Key == ConsoleKey.RightArrow)
                {
                    if (Console.CursorLeft <= read_buffer.Length)
                    {
                        cursor++;
                    }
                    else
                    {
                        Console.Beep();
                    }
                }
                else if (valid_chars.Contains(k.KeyChar))
                {
                    cursor++;
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
