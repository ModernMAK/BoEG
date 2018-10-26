using System.IO;
using UnityEngine;

namespace Networking.Messages
{
    public static class Messanger
    {
        public static MessageType CheckMessageType(byte[] data)
        {
            //First byte should be the message type, if its not then we want an exception thrown

            return (MessageType) data[0];
        }

        public static byte[] WriteDisconnectResponse(ulong salt)
        {
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                {
                    writer.Write((byte) MessageType.Disconnect);
                    writer.Write(salt);
                }
                return stream.ToArray();
            }
        }

        public static void ReadDisonnectResponse(byte[] data, out ulong salt)
        {
            using (var stream = new MemoryStream(data))
            using (var reader = new BinaryReader(stream))
            {
                var msgType = (MessageType) reader.ReadByte(); //Message
                Debug.Assert(msgType == MessageType.Disconnect);
                salt = reader.ReadUInt64();
            }
        }
    }
}