using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace Domain.Network
{
    public delegate void ReceiveAsyncCompleteEvent(object packet);

    public class AsyncReceiver : IDisposable
    {
        private const int ReceiveBufferSize = 1024;

        private Socket socket;
        private IPacketSerializer packetSerializer;

        private byte[] receiveBuffer;
        private int receiveLength;
        private SocketAsyncEventArgs receiveArgs;

        public AsyncReceiver(Socket socket, IPacketSerializer packetSerializer)
        {
            this.socket = socket;
            this.packetSerializer = packetSerializer;
        }

        public event ReceiveAsyncCompleteEvent OnReceived;
        public event SocketDisconnectHandler Disconnect;

        public void Dispose()
        {
            socket = null;
            receiveArgs?.Dispose();
            receiveArgs = null;
        }

        public void OnAccept()
        {
            receiveBuffer = new byte[ReceiveBufferSize];
            receiveLength = 0;
            receiveArgs = new SocketAsyncEventArgs();
            receiveArgs.SetBuffer(receiveBuffer, 0, receiveBuffer.Length);
            receiveArgs.Completed += OnReceiveComplete;

            var capturedContext = SynchronizationContext.Current;
            SynchronizationContext.SetSynchronizationContext(null);
            try
            {
                ReceiveAsync();
            }
            finally
            {
                SynchronizationContext.SetSynchronizationContext(capturedContext);
            }
        }

        private void OnReceiveComplete(object sender, SocketAsyncEventArgs args)
        {
            if (!socket?.Connected ?? true)
                return;

            if (args.SocketError != SocketError.Success)
            {
                Disconnect(args.SocketError);
                return;
            }

            var len = args.BytesTransferred;
            if (len == 0)
            {
                Console.WriteLine($"{DateTime.Now} graceful close");
                Disconnect(SocketError.Shutdown);
                return;
            }


            receiveLength += len;

            Console.WriteLine($"{DateTime.Now} OnReceiveComplete, {len}");

            using (var stream = new MemoryStream(receiveBuffer, 0, receiveLength, false, true))
            {
                var readOffset = 0;
                while (true)
                {
                    var packetLen = packetSerializer.PeekLength(stream);
                    if (packetLen == 0 || receiveLength - readOffset < packetLen)
                    {
                        receiveLength -= readOffset;

                        if (receiveLength > 0)
                        {
                            Array.Copy(receiveBuffer, readOffset,
                                       receiveBuffer, 0, receiveLength);
                        }

                        break;
                    }

                    var packet = packetSerializer.Deserialize(stream);

                    if (OnReceived != null)
                    {
                        OnReceived(packet);
                    }

                    readOffset += packetLen;
                }
            }

            ReceiveAsync();
        }

        private void ReceiveAsync()
        {
            if (!socket?.Connected ?? true)
                return;

            // 카운트 조정
            receiveArgs.SetBuffer(receiveLength, receiveBuffer.Length - receiveLength);

            try
            {
                if (!socket.ReceiveAsync(receiveArgs))
                    OnReceiveComplete(socket, receiveArgs);
            }
            catch (SocketException ex)
            {
                Disconnect(ex.SocketErrorCode);
            }
            catch (ObjectDisposedException)
            {
                Disconnect(SocketError.NotConnected);
            }
            catch (Exception)
            {
                Disconnect();
            }
        }
    }
}
