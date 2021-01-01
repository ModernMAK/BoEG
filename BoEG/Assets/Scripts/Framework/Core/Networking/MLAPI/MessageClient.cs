using System;
using System.IO;
using System.Text;
using Framework.Core.Serialization;

namespace Framework.Core.Networking.MLAPI
{
    public class MessageClient : AbstractMessageLayer
    {
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
            using (var buffer = new MemoryStream())
            {
                if (_client.ReadMessage(buffer))
                {
                    using (var reader = new BinaryReader(buffer, Encoding))
                    {
                        var deserializer = new BinaryDeserializer(reader);
                        message = new Message();
                        message.Deserialize(deserializer);
                        Received.Invoke(message);
                        return true;
                    }
                }
            }

            message = default;
            return false;
        }

        public bool WriteMessage(Message message)
        {
            using (var buffer = new MemoryStream())
            {
                using (var writer = new BinaryWriter(buffer, Encoding))
                {
                    var serializer = new BinarySerializer(writer);
                    message.Serialize(serializer);
                    if (_client.WriteMessage(buffer))
                    {
                        Sent.Invoke(message);
                        return true;
                    }

                    return false;
                }
            }
        }
    }
}