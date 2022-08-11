using System.IO;

namespace Domain.Network
{
    public interface IPacketSerializer
    {
        void Serialize(MemoryStream stream, Packet packet);

        int PeekLength(MemoryStream stream);
        object Deserialize(MemoryStream stream);
    }
}
