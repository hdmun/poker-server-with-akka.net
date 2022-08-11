using Domain.Network;
using System.Net;

namespace Server.Gateway
{
    public class GatewaySettings
    {
        public IPEndPoint LocalEndPoint { get; set; }

        public IPacketSerializer PacketSerializer { get; set; }
    }
}
