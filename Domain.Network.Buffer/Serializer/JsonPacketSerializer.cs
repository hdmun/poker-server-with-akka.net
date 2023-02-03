using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace Domain.Network.Buffer.Serializer
{
    public class JsonPacketSerializer : IPacketSerializer
    {
        private readonly int lenSize = 4;

        private readonly Dictionary<string, Type> types;

        public JsonPacketSerializer(Dictionary<string, Type> types)
        {
            this.types = types;
        }

        public void Serialize(MemoryStream stream, Packet packet)
        {
            stream.Seek(lenSize, SeekOrigin.Current);

            var startPos = stream.Position;
            string typeName = packet.Message.GetType().Name;
            stream.WriteString(typeName);

            byte[] serialized = JsonSerializer.SerializeToUtf8Bytes(packet.Message);
            stream.Write(serialized);

            var endPosition = stream.Position;
            var len = (int)(endPosition - startPos);
            stream.Seek(0, SeekOrigin.Begin);
            stream.WriteInt(len);
            stream.Seek(endPosition, SeekOrigin.Current);
        }

        public int PeekLength(MemoryStream stream)
        {
            var len = (int)(stream.Length - stream.Position);
            if (len < lenSize)
                return 0;

            var packetLen = stream.ReadInt();
            stream.Seek(-lenSize, SeekOrigin.Current);
            return packetLen + lenSize;
        }

        public object Deserialize(MemoryStream stream)
        {
            int len = stream.ReadInt();  // packet length (4byte)

            int lenTypeName = stream.ReadInt();  // typeName length (4byte)
            var bytes = new byte[lenTypeName];
            stream.Read(bytes, 0, lenTypeName);

            var typeName = Encoding.UTF8.GetString(bytes);
            if (!types.ContainsKey(typeName))
                return null;

            var startPos = (int)stream.Position;
            var jsonSize = len - lenSize - lenTypeName;
            byte[] jsonData = new byte[jsonSize];
            System.Buffer.BlockCopy(stream.GetBuffer(), startPos, jsonData, 0, jsonSize);

            return JsonSerializer.Deserialize(jsonData, types[typeName]);
        }

        private ArraySegment<byte> GetBuffers(MemoryStream ms, int pos, int length)
        {
            return new ArraySegment<byte>(ms.GetBuffer(), pos, length);
        }
    }
}
