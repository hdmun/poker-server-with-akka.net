using Domain.Network;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;

namespace Server.Gateway.Tests.Mock.Network
{
    public class TcpClientSender : IDisposable
    {
        private TcpClient tcpClient;
        private IPacketSerializer packetSerializer;

        private byte[] sendBuf = new byte[1024 * 128];
        private Queue<Packet> sendPacketQueue = new();
        private volatile bool sendProcessing;

        public TcpClientSender(TcpClient tcpClient, IPacketSerializer packetSerializer)
        {
            this.tcpClient = tcpClient;
            this.packetSerializer = packetSerializer;
        }

        public event SocketDisconnectHandler Disconnect;

        public void Dispose()
        {
            tcpClient = null;
            packetSerializer = null;
            sendBuf = null;
            sendPacketQueue?.Clear();
            sendPacketQueue = null;
        }

        public void SendPacket(Packet packet)
        {
            if (packet == null)
            {
                throw new ArgumentNullException("packet");
            }

            lock (sendPacketQueue)
                sendPacketQueue.Enqueue(packet);

            if (sendProcessing == false)
                ProcessSend();
        }

        private void ProcessSend()
        {
            lock (sendPacketQueue)
            {
                while (sendPacketQueue.Count > 0)
                {
                    Packet packet = sendPacketQueue.Dequeue();

                    int length;
                    using (var ms = new MemoryStream())
                    {
                        packetSerializer.Serialize(ms, packet);
                        length = (int)ms.Length;
                        if (length > sendBuf.Length)
                        {
                            Console.WriteLine($"ProcessSend got too large packet. Length={length}");
                            Disconnect();
                            return;
                        }
                        Array.Copy(ms.GetBuffer(), sendBuf, length);
                    }

                    try
                    {
                        sendProcessing = true;
                        tcpClient.GetStream().BeginWrite(sendBuf, 0, length, OnSend, null);
                        break;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"ProcessSend Exception, {ex}");
                        break;
                    }
                }
            }
        }

        private void OnSend(IAsyncResult ar)
        {
            try
            {
                tcpClient.GetStream().EndWrite(ar);
            }
            catch (SocketException ex)
            {
                Console.WriteLine($"OnSend SocketException, {ex}");
                Disconnect(ex.SocketErrorCode);
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"OnSend Exception, {ex}");
                Disconnect();
                return;
            }

            sendProcessing = false;
            ProcessSend();
        }
    }
}
