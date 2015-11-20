using NetDotNet.API.Results;
using NetDotNet.API.Requests;
using System.Net.Sockets;
using System.Net;
using System;
using System.Text;
using System.IO;

namespace NetDotNet.SocketLayer
{
    internal class HTTPConnection
    {
        private Socket sckt;
        private string remoteIP;

        internal HTTPConnection(Socket s)
        {
            sckt = s;
            remoteIP = ((IPEndPoint) sckt.RemoteEndPoint).Address.ToString() + ":" + ((IPEndPoint) sckt.RemoteEndPoint).Port.ToString();
            AcceptData();
            TimeoutScheduler.AddTimeout(this);
        }

        internal void Close()
        {
            try
            {
                sckt.Shutdown(SocketShutdown.Both);
                sckt.Close();
                sckt.Dispose();
            }
            catch (ObjectDisposedException) { }


            Listener.RemoveConnection(this);
        }

        private byte[] data = new byte[1];
        private void AcceptData()
        {
            try
            {
                sckt.BeginReceive(data, 0, data.Length, SocketFlags.None, new AsyncCallback(DataAccepted), null);
            }
            catch (SocketException se)
            {
                Core.Logger.Log(Core.LogLevel.Error, new[] { "Encountered SocketException when receiving data from " + remoteIP + "! Closing connection.",
                                                           "Details: " + se.Message });
                Close();
            }
            catch (ObjectDisposedException) { Close(); }
        }

        short bytesSoFar = 0;
        short contentLength = -1;
        string buffer = "";
        string requestRaw = "";
        private void DataAccepted(IAsyncResult r)
        {
            bytesSoFar++;
            if (bytesSoFar == contentLength)
            {
                RequestReceived(new Request(requestRaw), Serve);
                return;
            }

            requestRaw += Encoding.UTF8.GetString(data);

            if (data[0] == 99)
            {
                string[] parts = buffer.Split(':');
                if (parts.Length == 2)
                {
                    if (parts[0] == "Content-Length")
                    {
                        if (!short.TryParse(parts[1].Trim(), out contentLength))
                        {
                            // oh shit
                        }

                        if (contentLength > Core.ServerProperties.MaxRequestLength)
                        {
                            Core.Logger.Log()
                            Close();
                        }
                    }
                }

                buffer = "";
            }
            else
            {
                buffer += Encoding.UTF8.GetString(data);
            }

            try
            {
                sckt.EndReceive(r);
            }
            catch (SocketException se)
            {
                Core.Logger.Log(Core.LogLevel.Error, new[] { "Encountered SocketException when receiving data from " + remoteIP + "! Closing connection.",
                                                           "Details: " + se.Message });
                Close();
            }
            catch (ObjectDisposedException) { Close(); }
            AcceptData();
        }

        private void Serve(Result result)
        {
            using (StreamReader stream = result.Body.GetStream())
            {
                while (! stream.EndOfStream)
                {
                    sckt.Send(new[] { (byte)stream.Read() });
                }
            }
        }

        internal delegate void RequestCompleteHandler(Result result);
        internal delegate void RequestHandler(Request request, RequestCompleteHandler callback);
        internal event RequestHandler RequestReceived;
    }
}
