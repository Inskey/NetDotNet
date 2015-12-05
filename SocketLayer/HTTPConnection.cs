using NetDotNet.API.Results;
using NetDotNet.API.Requests;
using System.Net.Sockets;
using System.Net;
using System;
using System.Text;
using System.IO;
using NetDotNet.Core;
using System.Threading;

namespace NetDotNet.SocketLayer
{
    internal class HTTPConnection
    {
        private Socket sckt;
        internal IPAddress RemoteIP;
        private string prefix;

        internal HTTPConnection(Socket s)
        {
            sckt = s;
            IPEndPoint ep = ((IPEndPoint)sckt.RemoteEndPoint);
            RemoteIP = ep.Address;
            prefix = "[" + ep.Address.ToString() + ":" + ep.Port.ToString() + "] ";
            AcceptData();
            TimeoutScheduler.AddTimeout(this);
        }

        internal void Timeout()
        {
            Logger.Log(LogLevel.Error, prefix + "Client took too long to send the request. Disconnecting.");
            Close();
        }

        internal void Close()
        {
            try
            {
                sckt.Shutdown(SocketShutdown.Both);
                sckt.Close();
            }
            catch (ObjectDisposedException) { }

            Listener.RemoveConnection(this);
            Logger.Log(prefix + "Connection closed.");
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
                Logger.Log(LogLevel.Error, new[] { prefix + "Encountered SocketException when receiving data! Closing connection.",
                                                   "Details: " + se.Message });
                Close();
            }
            catch (ObjectDisposedException) { Close(); }
        }

        short bytesSoFar = 0;
        short contentLength = -1;
        string line = "";
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
                string[] parts = line.Split(':');
                if (parts.Length == 2)
                {
                    if (parts[0] == "Content-Length")
                    {
                        if (!short.TryParse(parts[1].Trim(), out contentLength))
                        {
                            Logger.Log(LogLevel.Error, prefix + "Client sent bad Content-Length (not a valid number). Closing connection.");
                            Close();
                        }
                        else if (contentLength > ServerProperties.MaxRequestLength)
                        {
                            Logger.Log(LogLevel.Error + "Client sent bad Content-Length (larger than max-request-length set in ./server.properties). Closing connection.");
                            Close();
                        }
                        else if (contentLength < bytesSoFar)
                        {
                            Logger.Log(LogLevel.Error + "Client sent bad Content-Length (smaller than amount received so far). Closing connection.");
                            Close();
                        }
                    }
                }

                line = "";
            }
            else
            {
                line += Encoding.UTF8.GetString(data);
            }

            try
            {
                sckt.EndReceive(r);
            }
            catch (SocketException se)
            {
                Logger.Log(LogLevel.Error, new[] { prefix + "Encountered SocketException when receiving data! Closing connection.",
                                                   "Details: " + se.Message });
                Close();
            }
            catch (ObjectDisposedException ode)
            {
                Logger.Log(LogLevel.Error, new[] { prefix + "Encountered ObjectDisposedException when receiving data! Closing connection.",
                                                   "Details: " + ode.Message });
                Close();
            }
            AcceptData();
        }

        private void Serve(Result result)
        {
            sckt.Send(Encoding.ASCII.GetBytes(result.GetHeader()));

            using (StreamReader stream = result.Body.GetStream())
            {
                while (! stream.EndOfStream)
                {
                    for (byte i = 0; i < 10 && ! stream.EndOfStream; i++)
                    {
                        sckt.Send(new[] { (byte)stream.Read() });
                    }
                    Thread.Sleep(1);
                }
            }

            if (! result.Keep_Alive.Value)
            {
                Close();
            }
        }

        internal delegate void RequestCompleteHandler(Result result);
        internal delegate void RequestHandler(Request request, RequestCompleteHandler callback);
        internal event RequestHandler RequestReceived;
    }
}
