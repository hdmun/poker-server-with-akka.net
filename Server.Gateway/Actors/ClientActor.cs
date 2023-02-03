using Akka.Actor;
using Domain.Network;
using Domain.Network.AsyncSocket;
using Server.Gateway.Message;
using System;

namespace Server.Gateway.Actor
{
    public class ClientActor : UntypedActor
    {
        private readonly IActorRef self;
        private readonly AsyncSession session;

        public ClientActor(AsyncSession session)
        {
            self = Self;
            this.session = session;
            this.session.OnReceived += Session_OnReceived;
            this.session.OnAccept();
        }

        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case LoginMessage loginMessage:
                    OnLoginMessage(loginMessage);
                    break;
                default:
                    Console.WriteLine($"Invalid message type, {message.GetType()}");
                    break;
            }
        }

        private void Session_OnReceived(object packet)
        {
            self.Tell(packet, self);
        }

        private void OnLoginMessage(LoginMessage message)
        {
            session.SendPacket(new LoginResponseMessage
            {
                Authenticated = true,
                Id = message.Id,
                Name = "Name"
            });
        }
    }
}
