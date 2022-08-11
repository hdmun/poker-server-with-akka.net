using System;
using System.IO;
using System.Text;

namespace Domain.Network.Buffer.Serializer
{
    public static class SerializeExtensions
    {
        public static int ReadInt(this Stream stream)
        {
            var intBytes = new byte[4];
            stream.Read(intBytes);
            return BitConverter.ToInt32(intBytes, 0);
        }

        public static void WriteInt(this Stream stream, int value)
        {
            var bytes = BitConverter.GetBytes(value);
            stream.Write(bytes);
        }

        public static string ReadString(this Stream stream)
        {
            int lenStr = stream.ReadInt();  // string length

            var bytes = new byte[lenStr];
            stream.Read(bytes, 0, lenStr);
            return Encoding.UTF8.GetString(bytes);
        }

        public static void WriteString(this Stream stream, string value)
        {
            var bytes = Encoding.UTF8.GetBytes(value);
            stream.WriteInt(bytes.Length);
            stream.Write(bytes);
        }
    }
}
