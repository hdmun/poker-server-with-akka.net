using Akka.Actor;
using Domain.Network.Buffer.Serializer;
using Server.Gateway.Actors;
using System.Linq;
using System.Net;
using System.Reflection;
using Topshelf;

namespace Server.Gateway
{
    public class GatewayService : ServiceControl
    {
        private ActorSystem actorSystem;
        private IActorRef gatewayActorRef;

        public GatewayService()
        {
        }

        bool ServiceControl.Start(HostControl hostControl)
        {
            var messageTypes = Assembly.Load("Server.Gateway.Message")
                .GetTypes()
                .ToDictionary(t => t.Name, t => t);

            var gatewaySettings = new GatewaySettings()
            {
                LocalEndPoint = new IPEndPoint(IPAddress.Any, 5000),
                PacketSerializer = new JsonPacketSerializer(messageTypes)
            };

            actorSystem = ActorSystem.Create("PokerServer");
            gatewayActorRef = actorSystem.ActorOf(Props.Create(() => new GatewayActor(gatewaySettings)), "GatewayActor");

            return true;
        }

        bool ServiceControl.Stop(HostControl hostControl)
        {
            return true;
        } 
    }
}
