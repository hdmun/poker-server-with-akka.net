using Domain.Network;
using System;
using System.Net;
using System.Net.Sockets;

namespace Server.Gateway.Tests.Mock.Network
{
    public delegate void TcpConnectionConnectedEvent(TcpConnection tcpConnection);
    public delegate void TcpConnectionClosedEvent(TcpConnection tcpConnection);

    public class TcpConnection
    {
        private TcpClient tcpClient;
        private EndPoint remoteEndPoint;

        private TcpClientSender sender;
        private TcpClientReceiver receiver;

        public TcpConnection(IPacketSerializer packetSerializer)
        {
            tcpClient = new TcpClient();

            sender = new TcpClientSender(tcpClient, packetSerializer);
            receiver = new TcpClientReceiver(tcpClient, packetSerializer);
        }

        public event TcpConnectionConnectedEvent OnConnected;
        public event TcpConnectionClosedEvent OnClosed;
        public event TcpClientReceivedEvent OnReceived;

        private void Close()
        {
            if (tcpClient.Connected)
                tcpClient.Close();

            Console.WriteLine($"{DateTime.Now} TcpConnection.Close, {remoteEndPoint}");
        }

        public void Connect(IPEndPoint remoteEp)
        {
            sender.Disconnect += Disconnect;
            receiver.OnReceived += OnReceived;
            receiver.Disconnect += Disconnect;

            remoteEndPoint = remoteEp;

            try
            {
                tcpClient.Connect(remoteEp.Address, remoteEp.Port);
                remoteEndPoint = tcpClient.Client.RemoteEndPoint;
            }
            catch (SocketException ex)
            {
                Console.WriteLine($"{DateTime.Now} Connect Failed {ex.Message}, {remoteEndPoint}, {ex.SocketErrorCode}");
                Disconnect(ex.SocketErrorCode);
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{DateTime.Now} Connect Failed {ex.Message}, {remoteEndPoint}");
                Disconnect();
                return;
            }

            Console.WriteLine($"{DateTime.Now} ProxyClient.OnConnected, {remoteEndPoint}");

            OnConnected?.Invoke(this);

            receiver.ProcessRecv();
        }

        public void Disconnect(SocketError socketError = SocketError.Success)
        {
            Close();

            OnClosed?.Invoke(this);
        }

        public void SendPacket(Packet packet) => sender.SendPacket(packet);
    }
}
