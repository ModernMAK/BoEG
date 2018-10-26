using System.IO;
using UnityEngine;

namespace Networking.Messages
{
    public class ConnectMessage : Message
    {
        public ConnectMessage()
        {
            Salt = 0;
        }
        public ConnectMessage(ulong salt)
        {
            Salt = salt;
        }

        public ulong Salt { get; private set; }

        public override MessageType MessageType
        {
            get { return MessageType.Connect; }
        }

        public override byte[] Write()
        {
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                {
                    writer.Write((byte) MessageType);
                    writer.Write(Salt);
                }
                return stream.ToArray();
            }
        }

        public override void Read(byte[] data)
        {
            using (var stream = new MemoryStream(data))
            using (var reader = new BinaryReader(stream))
            {
                var msgType = (MessageType) reader.ReadByte(); //Message
                Debug.Assert(msgType == MessageType);
                Salt = reader.ReadUInt64();
            }
        }
    }
    public class DisconnectMessage : Message
    {
        public DisconnectMessage()
        {
        }

        public override MessageType MessageType
        {
            get { return MessageType.Disconnect; }
        }

        public override byte[] Write()
        {
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                {
                    writer.Write((byte) MessageType);
                }
                return stream.ToArray();
            }
        }

        public override void Read(byte[] data)
        {
            using (var stream = new MemoryStream(data))
            using (var reader = new BinaryReader(stream))
            {
                var msgType = (MessageType) reader.ReadByte(); //Message
                Debug.Assert(msgType == MessageType);
            }
        }
    }
}