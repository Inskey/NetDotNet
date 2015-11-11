using System.Net.Sockets;
using System.Net;
using System;
using System.Collections.Generic;


namespace NetDotNet.SocketLayer
{
    internal class Listener
    {
        private Socket s;
        private List<HTTPConnection> connections = new List<HTTPConnection>();

        internal Listener()
        {
            s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            remove = c => connections.Remove(c);
        }

        internal void Open(IPAddress address = null, int port = 80)
        {
            if (address == null)
            {
                address = IPAddress.Parse("127.0.0.1");
            }
            s.Bind(new IPEndPoint(address, port));
            s.Listen(10);
            AcceptConnection();
        }

        internal void Close()
        {
            foreach (HTTPConnection conn in connections)
            {
                conn.Close();
            }

            try
            {
                s.Close();
                s.Dispose();
            }
            catch (ObjectDisposedException) { }
        }

        private static Action<HTTPConnection> remove;
        internal static void RemoveConnection(HTTPConnection c)
        {
            remove(c);
        }

        private void AcceptConnection()
        {
            try
            {
                s.BeginAccept(ConnectedCallback, s);
            }
            catch (ObjectDisposedException) { }
        }

        private void ConnectedCallback(IAsyncResult r)
        {
            HTTPConnection conn;
            try
            {
                conn = new HTTPConnection(s.EndAccept(r));
                Core.EntryPoint.SubscribeConnection(conn);
            }
            catch (ObjectDisposedException) { return; }

            connections.Add(conn);

            AcceptConnection();
        }
    }
}
