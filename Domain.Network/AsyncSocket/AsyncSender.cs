using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace Domain.Network
{
    public class AsyncSender : IDisposable
    {
        private const int SendBufferSize = 1024;

        private Socket socket;
        private IPacketSerializer packetSerializer;

        private byte[] sendBuffer;
        private int sendOffset;
        private int sendLength;
        private SocketAsyncEventArgs sendArgs;

        private int sendCount;
        private ConcurrentQueue<Packet> sendQueue;

        public AsyncSender(Socket socket, IPacketSerializer packetSerializer)
        {
            this.socket = socket;
            this.packetSerializer = packetSerializer;
        }

        public event SocketDisconnectHandler Disconnect;

        public void Dispose()
        {
            socket = null;
            sendArgs?.Dispose();
            sendArgs = null;
        }

        public void OnAccept()
        {
            sendBuffer = new byte[SendBufferSize];
            sendOffset = 0;
            sendLength = 0;
            sendArgs = new SocketAsyncEventArgs();
            sendArgs.SetBuffer(sendBuffer, 0, sendBuffer.Length);
            sendArgs.Completed += OnSendComplete;

            sendCount = 0;
            sendQueue = new ConcurrentQueue<Packet>();
        }

        private void OnSendComplete(object sender, SocketAsyncEventArgs args)
        {
            if (args.SocketError != SocketError.Success)
            {
                Disconnect(args.SocketError);
                return;
            }

            sendOffset += args.BytesTransferred;
            if (sendOffset < sendLength)
            {
                SendAsync();
                return;
            }

            if (Interlocked.Decrement(ref sendCount) > 0)
            {
                Packet packet;
                while (sendQueue.TryDequeue(out packet) == false)
                {
                }

                if (packet != null)
                    TrySend(packet);
                else
                    Disconnect();
            }
        }

        public void Send(object message)
        {
            var packet = Packet.Create(message);

            if (Interlocked.Increment(ref sendCount) == 1)
            {
                var oldContext = SynchronizationContext.Current;
                SynchronizationContext.SetSynchronizationContext(null);

                try
                {
                    TrySend(packet);
                }
                finally
                {
                    SynchronizationContext.SetSynchronizationContext(oldContext);
                }
            }
            else
            {
                sendQueue.Enqueue(packet);
            }
        }

        private void TrySend(Packet packet)
        {
            var stream = new MemoryStream(sendBuffer, 0, sendBuffer.Length, true);
            packetSerializer.Serialize(stream, packet);

            sendOffset = 0;
            sendLength = (int)stream.Position;

            SendAsync();
        }

        private void SendAsync()
        {
            sendArgs.SetBuffer(sendOffset, sendLength - sendOffset);

            try
            {
                if (!socket.SendAsync(sendArgs))
                    OnSendComplete(socket, sendArgs);
            }
            catch (SocketException)
            {
                // ex.SocketErrorCode;
            }
            catch (ObjectDisposedException)
            {
                // SocketError.NotConnected
            }
            catch (Exception)
            {
                // log
            }
        }
    }
}
