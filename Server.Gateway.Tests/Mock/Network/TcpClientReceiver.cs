using Domain.Network;
using System;
using System.IO;
using System.Net.Sockets;

namespace Server.Gateway.Tests.Mock.Network
{
    public delegate void TcpClientReceivedEvent(object packet);

    public class TcpClientReceiver : IDisposable
    {
        private TcpClient tcpClient;
        private IPacketSerializer packetSerializer;

        private byte[] recvBuf = new byte[1024 * 128];
        private int recvBufLen;

        public TcpClientReceiver(TcpClient tcpClient, IPacketSerializer packetSerializer)
        {
            this.tcpClient = tcpClient;
            this.packetSerializer = packetSerializer;
        }

        public event TcpClientReceivedEvent OnReceived;
        public event SocketDisconnectHandler Disconnect;

        public void Dispose()
        {
            tcpClient = null;

            GC.SuppressFinalize(this);
        }

        public void ProcessRecv()
        {
            try
            {
                tcpClient.GetStream().BeginRead(
                    recvBuf, recvBufLen, recvBuf.Length - recvBufLen,
                    OnReceive, null);
            }
            catch (SocketException ex)
            {
                Console.WriteLine($"ProcessRecv SocketException, {ex}");
                Disconnect(ex.SocketErrorCode);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ProcessRecv Exception, {ex}");
                Disconnect();
            }
        }

        private void OnReceive(IAsyncResult ar)
        {
            try
            {
                int len = tcpClient.GetStream().EndRead(ar);
                if (len == 0)
                {
                    Console.WriteLine($"OnReceive zero-byte");
                    Disconnect();
                    return;
                }

                recvBufLen += len;
            }
            catch (SocketException ex)
            {
                Console.WriteLine($"OnReceive Exception, {ex}");
                Disconnect(ex.SocketErrorCode);
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"OnReceive Exception, {ex}");
                Disconnect();
                return;
            }

            while (true)
            {
                using (var stream = new MemoryStream(recvBuf, 0, recvBufLen, false, true))
                {
                    var length = packetSerializer.PeekLength(stream);
                    if (length > recvBuf.Length)
                    {
                        Console.WriteLine($"OnReceive got too large packet. Length={length}");
                        Disconnect();
                        return;
                    }
                    if (length == 0 || recvBufLen < length)
                        break;

                    try
                    {
                        var packet = packetSerializer.Deserialize(stream);
                        if (OnReceived != null)
                            OnReceived(packet);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Deserialize Error, {ex}");
                        Disconnect();
                        return;
                    }

                    int leftLen = recvBufLen - length;
                    if (leftLen > 0)
                    {
                        Array.Copy(recvBuf, length, recvBuf, 0, leftLen);
                        recvBufLen = leftLen;
                    }
                    else
                    {
                        recvBufLen = 0;
                    }
                }
            }

            ProcessRecv();
        }
    }
}
