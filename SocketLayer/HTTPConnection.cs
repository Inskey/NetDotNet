using NetDotNet.API.Results;
using NetDotNet.API.Requests;
using System.Net.Sockets;
using System.Net;
using System;
using System.Text;
using System.IO;
using NetDotNet.Core;
using NetDotNet.Core.Expiration;
using System.Threading;

namespace NetDotNet.SocketLayer
{
    internal class HTTPConnection : IExpirable
    {
        private Socket sckt;
        internal IPAddress RemoteIP;
        private string prefix;

        private DateTime expiration;

        internal HTTPConnection(Socket s)
        {
            sckt = s;
            IPEndPoint ep = ((IPEndPoint) sckt.RemoteEndPoint);
            RemoteIP = ep.Address;
            prefix = "[" + ep.Address.ToString() + ":" + ep.Port.ToString() + "] ";
            expiration = DateTime.Now + TimeSpan.FromSeconds(ServerProperties.);
            AcceptData();
        }

        internal void Close()
        {
            try
            {
                sckt.Shutdown(SocketShutdown.Both);
                sckt.Close(0);
            }
            catch (ObjectDisposedException) { }

            Listener.RemoveConnection(this);
            Logger.Log(prefix + "Connection closed.");
        }

        private byte[] data = new byte[1];
        private void AcceptData(bool skip = false)
        {
            try
            {
                sckt.BeginReceive(data, 0, data.Length, SocketFlags.None, skip ? (AsyncCallback) Skip : DataAccepted, null);
            }
            catch (SocketException se)
            {
                Logger.Log(LogLevel.Error, new[] { prefix + "Encountered SocketException when receiving data! Closing connection.",
                                                   "Details: " + se.Message });
                Close();
            }
            catch (ObjectDisposedException) { Close(); }
        }

        private void Skip(IAsyncResult r)
        {
            EndReceive(r);
        }

        ushort headerLength = 0;
        ulong bytesSoFar = 0;
        bool first = true;
        bool inBody = false;
        RequestType type;
        ulong contentLength;
        string line = "";
        string requestRaw = "";
        private void DataAccepted(IAsyncResult r)
        {
            requestRaw += Encoding.UTF8.GetString(data);

            if (inBody)
            {
                bytesSoFar++;
                if (bytesSoFar == contentLength)
                {
                    inBody = false;
                    RequestReceived(new Request(requestRaw), Serve);
                    bytesSoFar = 0;
                    first = true;
                    inBody = false;
                    line = "";
                    requestRaw = "";
                    return;
                }
            }
            else // If we're not in the body, we must be in the header, so parse some necessary things. 
            {
                headerLength++;
                if (headerLength > ServerProperties.MaxHeaderLength)
                {
                    Logger.Log(LogLevel.Error, prefix + "Client exceeded max header length! Is this an attack? Closing connection.");
                    Close();
                    return;
                }
                if (data[0] != 10) // Ignore \n because it will always follow a \r.
                {
                    if (data[0] == 13) // carriage return (\r)
                    {
                        if (first) // if this is the first line, get the request type and check if it's trying to access an upload token
                        {
                            switch (line.Split(' ')[0])
                            {
                                case "GET":
                                    type = RequestType.GET;
                                    break;
                                case "POST":
                                    type = RequestType.POST;
                                    break;
                                case "OPTIONS":
                                    type = RequestType.OPTIONS;
                                    break;
                                case "HEAD":
                                    type = RequestType.HEAD;
                                    break;
                                case "TRACE":
                                    type = RequestType.TRACE;
                                    break;
                                default: // This case could be handled by the parser, but better to catch it early.
                                    Logger.Log(LogLevel.Error, "Client sent unsupported HTTP method: " + line.Split(' ')[0] + "! Closing connection!");
                                    Close();
                                    return;
                            }
                            first = false;
                        }
                        else
                        {
                            if (line == "") // If it's a blank line, we're now in the body
                            {
                                inBody = true;
                                headerLength = 0;
                                AcceptData(true);
                                return;
                            }

                            string[] parts = line.Split(':');
                            if (parts.Length == 2)
                            {
                                if (parts[0] == "Content-Length")
                                {
                                    if (! ulong.TryParse(parts[1].Trim(), out contentLength))
                                    {
                                        Logger.Log(LogLevel.Error, prefix + "Client sent bad Content-Length (not a valid number). Closing connection.");
                                        Close();
                                        return;
                                    }
                                    else if (contentLength > ServerProperties.MaxPostLength)
                                    {
                                        Logger.Log(LogLevel.Error + "Client sent bad Content-Length (larger than max-request-length set in ./server.properties). Closing connection.");
                                        Close();
                                        return;
                                    }
                                }
                            }
                        }

                        line = "";
                    }
                    else
                    {
                        line += Encoding.UTF8.GetString(data);
                    }
                }
            }

            EndReceive(r);
        }

        private void EndReceive(IAsyncResult r)
        {
            try
            {
                sckt.EndReceive(r);
            }
            catch (SocketException se)
            {
                Logger.Log(LogLevel.Error, new[] { prefix + "Encountered SocketException when receiving data! Closing connection.",
                                                   "Details: " + se.Message });
                Close();
                return;
            }
            catch (ObjectDisposedException ode)
            {
                Logger.Log(LogLevel.Error, new[] { prefix + "Encountered ObjectDisposedException when receiving data! Closing connection.",
                                                   "Details: " + ode.Message });
                Close();
                return;
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
                    byte[] data = new byte[10];
                    for (ushort i = 0; i < ServerProperties.BytesPerPckt && ! stream.EndOfStream; i++)
                    {
                        data[i] = (byte) stream.Read();
                    }
                    sckt.Send(data);
                    Thread.Sleep(ServerProperties.PcktDelay);
                }
            }

            if (! result.Keep_Alive.Value)
            {
                Close();
            }
        }

        void IExpirable.Expire(bool early)
        {
            Logger.Log(LogLevel.Error, prefix + "Client took too long to send the request. Disconnecting.");
            Close();
        }

        DateTime? IExpirable.GetExpiration()
        {
            return expiration;
        }

        internal delegate void RequestCompleteHandler(Result result);
        internal delegate void RequestHandler(Request request, RequestCompleteHandler callback);
        internal event RequestHandler RequestReceived;
    }
}
