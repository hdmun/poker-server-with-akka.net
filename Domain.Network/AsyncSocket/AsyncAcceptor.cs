using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Domain.Network.AsyncSocket
{
    public class AsyncAcceptor
    {
        private Socket listenSock;
        private Stack<SocketAsyncEventArgs> acceptArgsPool;

        public AsyncAcceptor()
        {
            listenSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            acceptArgsPool = new Stack<SocketAsyncEventArgs>();
        }

        public event OnAcceptEvent OnAccept;

        public void BeginAcceptor(IPEndPoint localEndPoint, int workerCount = 10)
        {
            try
            {
                listenSock.Bind(localEndPoint);
                listenSock.Listen((int)SocketOptionName.MaxConnections);

                for (var i = 0; i < workerCount; i++)
                {
                    var acceptArg = new SocketAsyncEventArgs();
                    acceptArg.Completed += OnIOCompleted;

                    lock (acceptArgsPool)
                        acceptArgsPool.Push(acceptArg);
                }

                AcceptAsyncPool();
            }
            catch (Exception)
            {
                // logging
                throw;
            }
        }

        private void OnIOCompleted(object sender, SocketAsyncEventArgs e)
        {
            switch (e.LastOperation)
            {
                case SocketAsyncOperation.Accept:
                    OnAcceptComplete(e);
                    break;
                default:
                    // logging
                    break;
            }
        }

        private void OnAcceptComplete(SocketAsyncEventArgs args)
        {
            var acceptSocket = args.AcceptSocket;
            args.AcceptSocket = null;

            AcceptAsyncPool();

            if (args.SocketError != SocketError.Success)
            {
                // logging
                return;
            }

            if (acceptSocket != null)
            {
                OnAccept?.Invoke(acceptSocket);
            }

            lock (acceptArgsPool)
                acceptArgsPool.Push(args);
        }

        private void AcceptAsyncPool()
        {
            SocketAsyncEventArgs acceptArg;
            lock (acceptArgsPool)
                acceptArg = (acceptArgsPool.Count > 1) ? acceptArgsPool.Pop() : new SocketAsyncEventArgs();

            AcceptAsync(acceptArg);
        }

        private void AcceptAsync(SocketAsyncEventArgs args)
        {
            bool isPending = listenSock.AcceptAsync(args);
            if (!isPending)
            {
                OnIOCompleted(listenSock, args);
            }
        }

        public void Close()
        {
            listenSock?.Close();
            acceptArgsPool?.Clear();

            listenSock = null;
            acceptArgsPool = null;
        }
    }
}
