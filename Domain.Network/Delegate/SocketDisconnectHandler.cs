using System.Net.Sockets;

namespace Domain.Network
{
    public delegate void SocketDisconnectHandler(SocketError socketError = SocketError.Success);
}
