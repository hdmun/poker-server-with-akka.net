using Domain.Network;
using Domain.Network.Buffer.Serializer;
using Server.Gateway.Message;
using Server.Gateway.Tests.Mock.Network;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace Server.Gateway.Tests.Mock
{
    public class MockClient
    {
        private TcpConnection connection;
        private bool connected;

        public string Id { get; private set; }
        public string Name { get; private set; }

        public MockClient()
        {
            var messageTypes = Assembly.Load("Server.Gateway.Message")
                .GetTypes()
                .ToDictionary(t => t.Name, t => t);

            connection = new TcpConnection(new JsonPacketSerializer(messageTypes));
            connection.OnConnected += Connection_OnConnected;
            connection.OnClosed += Connection_OnClosed;
            connection.OnReceived += Connection_OnReceived;

            connected = false;

            Id = null;
            Name = null;
        }

        public async Task<bool> Connect()
        {
            if (connected) return false;

            connected = false;
            connection.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5000));

            await Task.Run(async () =>
            {
                while (!connected) await Task.Delay(1);
            });

            return connected;
        }

        public void Disconnect() => connection.Disconnect();

        public async Task SendLoginPacketAsync(string loginId, string password)
        {
            connection.SendPacket(Packet.Create(new LoginMessage
            {
                Id = loginId,
                Password = password
            }));

            // wait response
            await Task.Run(async () =>
            {
                while (Id == null && Name == null) await Task.Delay(1);
            });
        }

        private void Connection_OnConnected(TcpConnection tcpConnection)
        {
            connected = true;
        }

        private void Connection_OnClosed(TcpConnection tcpConnection)
        {
        }

        private void Connection_OnReceived(object packet)
        {
            switch (packet)
            {
                case LoginResponseMessage loginResponse:
                    Id = loginResponse.Id;
                    Name = loginResponse.Name;
                    break;
                default:
                    break;
            }
        }
    }
}
