using System;
using System.IO;
using System.Text;
using MobaGame.Framework.Core.Networking.LLAPI;
using MobaGame.Framework.Core.Serialization;

namespace MobaGame.Framework.Core.Networking.MLAPI
{
    public class MessageClient
    {
        public MessageEventList Sent { get; }
        public MessageEventList Received { get; }

        private readonly NetworkClient _client;
        private readonly Encoding _encoding;

        public MessageClient() : this(new NetworkClient())
        {
        }

        public MessageClient(NetworkClient client) : this(client, Encoding.UTF8)
        {
        }

        public MessageClient(NetworkClient client, Encoding encoding)
        {
            _client = client;
            _encoding = encoding;

            Sent = new MessageEventList();
            Received = new MessageEventList();
        }

        public Encoding Encoding => _encoding;

        public event EventHandler Started
        {
            add => _client.ClientConnected += value;
            remove => _client.ClientConnected -= value;
        }

        public event EventHandler Stopped
        {
            add => _client.ClientDisconnected += value;
            remove => _client.ClientDisconnected -= value;
        }

        public Message ReadMessage() => TryReadMessage(out var message) ? message : null;

        public bool TryReadMessage(out Message message)
        {
            message = default;
            using (var buffer = new MemoryStream())
            {
                if (!_client.Connected)
                    return false;

                if (!_client.ReadMessage(buffer))
                    return false;

                buffer.Seek(0, SeekOrigin.Begin);
                using (var reader = new BinaryReader(buffer, Encoding))
                {
                    var deserializer = new BinaryDeserializer(reader);
                    message = new Message();
                    message.Deserialize(deserializer);
                    // ReceivedFrom.Invoke(new MessageSenderEventArgs(guid,message));
                    Received.Invoke(message);
                    return true;
                }
            }
        }

        public bool WriteMessage(Message message)
        {
            using (var buffer = new MemoryStream())
            {
                using (var writer = new BinaryWriter(buffer, Encoding))
                {
                    var serializer = new BinarySerializer(writer);
                    message.Serialize(serializer);
                    buffer.Seek(0, SeekOrigin.Begin);
                    if (_client.WriteMessage(buffer))
                    {
                        Sent.Invoke(message);
                        return true;
                    }

                    return false;
                }
            }
        }

        public void ReadAllMessages()
        {
            while (TryReadMessage(out _))
            {
            }
        }
    }
}