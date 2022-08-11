using Akka.Actor;
using Akka.TestKit.Xunit;
using Domain.Network.Buffer.Serializer;
using Server.Gateway.Actors;
using Server.Gateway.Tests.Mock;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace Server.Gateway.Tests
{
    public class ClientPacketTest : TestKit
    {
        private readonly GatewaySettings gatewaySettings = new GatewaySettings()
        {
            LocalEndPoint = new IPEndPoint(IPAddress.Any, 5000),
            PacketSerializer = new JsonPacketSerializer(
                Assembly.Load("Server.Gateway.Message")
                    .GetTypes()
                    .ToDictionary(t => t.Name, t => t)
                )
        };

        [Fact]
        public async Task SendLoginPacketTest()
        {
            var gatewayActorRef = this.Sys.ActorOf(Props.Create(() => new GatewayActor(gatewaySettings)));

            var mockClient = new MockClient();
            var connected = await mockClient.Connect();
            Assert.True(connected);

            await mockClient.SendLoginPacketAsync("testId", "testPassword");
            Assert.Equal("testId", mockClient.Id);
        }
    }
}
