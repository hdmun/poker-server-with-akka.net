using Akka.Actor;
using Domain.Network.AsyncSocket;
using Server.Gateway.Actor;
using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Server.Gateway.Actors
{
    public class GatewayActor : UntypedActor
    {
        private GatewaySettings gatewaySettings;
        private AsyncAcceptor acceptor;

        private HashSet<IActorRef> sessionSet;
        private IActorRef self;

        public GatewayActor(GatewaySettings gatewaySettings)
        {
            this.gatewaySettings = gatewaySettings;

            sessionSet = new HashSet<IActorRef>();
        }

        protected override void PreStart()
        {
            self = Self;  // iocp 스레드에서 접근하기 위해

            acceptor = new AsyncAcceptor();
            acceptor.OnAccept += Acceptor_OnAccept;
            acceptor.BeginAcceptor(gatewaySettings.LocalEndPoint);
        }

        private void Acceptor_OnAccept(Socket socket)
        {
            self.Tell(new AcceptMessage(socket), self);
        }

        class AcceptMessage
        {
            public Socket Socket { get; }

            public AcceptMessage(Socket socket)
            {
                Socket = socket;
            }
        }

        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case AcceptMessage accept:
                    OnAcceptMessage(accept);
                    break;
                default:
                    Console.WriteLine($"Invalid message type, {message}");
                    break;
            }
        }

        private void OnAcceptMessage(AcceptMessage message)
        {
            var session = new AsyncSession(message.Socket, gatewaySettings.PacketSerializer);
            var client = Context.ActorOf(Props.Create(() => new ClientActor(session)));
            if (client == null)
            {
                session.Disconnect();
                Console.WriteLine($"Deny a connection. (EndPoint={message.Socket.RemoteEndPoint})");
                return;
            }

            Context.Watch(client);  // actor 종료 될 때 까지 대기
            sessionSet.Add(client);
        }
    }
}
