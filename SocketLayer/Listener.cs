using System.Net.Sockets;
using System.Net;
using System;
using System.Collections.Generic;
using NetDotNet.Core;

namespace NetDotNet.SocketLayer
{
    internal class Listener
    {
        private Socket s;
        private List<HTTPConnection> connections = new List<HTTPConnection>();
        private Dictionary<IPAddress, byte> connsPerIP = new Dictionary<IPAddress, byte>();

        internal Listener()
        {
            s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            remove = c => RemoveConnection_(c);
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
            foreach (var conn in connections)
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
        internal static void RemoveConnection(HTTPConnection c) // static so that each HTTPConnection doesn't need a reference
        {
            remove(c);
        }

        private void RemoveConnection_(HTTPConnection c)
        {
            connections.Remove(c);
            connsPerIP[c.]
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
                var sckt = s.EndAccept(r);
                if (connsPerIP[((IPEndPoint) sckt.RemoteEndPoint).Address] == ServerProperties.MaxConnsPerIP)
                {
                    sckt.Shutdown(SocketShutdown.Both);
                    sckt.Close();
                }
                else
                {
                    conn = new HTTPConnection(sckt);
                    connections.Add(conn);
                    EntryPoint.SubscribeConnection(conn);
                }
            }
            catch (ObjectDisposedException)
            {

            }

            AcceptConnection();
        }
    }
}
