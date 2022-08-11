using System;
using System.Net.Sockets;

namespace Domain.Network.AsyncSocket
{
    public class AsyncSession
    {
        private Socket socket;
        private AsyncReceiver receiver;
        private AsyncSender sender;

        public AsyncSession(Socket socket, IPacketSerializer packetSerializer)
        {
            this.socket = socket;
            receiver = new AsyncReceiver(socket, packetSerializer);
            sender = new AsyncSender(socket, packetSerializer);
        }

        public event ReceiveAsyncCompleteEvent OnReceived;

        public void OnAccept()
        {
            receiver.Disconnect += Disconnect;
            receiver.OnReceived += OnReceived;
            sender.Disconnect += Disconnect;

            receiver.OnAccept();
            sender.OnAccept();
        }

        public void Disconnect(SocketError socketError = SocketError.Success)
        {
            Console.WriteLine("AsyncSession.Disconnect");
            socket.Close();
        }

        public void SendPacket(object message) => sender.Send(message);
    }
}
