using System;
using System.IO;
using System.Text;
using Framework.Core.Serialization;

namespace Framework.Core.Networking.MLAPI
{
    public class MessageServer : AbstractMessageLayer
    {
        private readonly NetworkServer _server;
        private readonly Encoding _encoding;

        public Encoding Encoding => _encoding;
        public NetworkServer InternalServer => _server;

        
        public event EventHandler Started
        {
            add => _server.ServerStarted += value;
            remove => _server.ServerStarted -= value;
        }
        public event EventHandler Stopped
        {
            add => _server.ServerStopped += value;
            remove => _server.ServerStopped -= value;
        }
        public event EventHandler<Guid> ClientConnected
        {
            add => _server.ClientConnected += value;
            remove => _server.ClientConnected -= value;
        }
        public event EventHandler<Guid> ClientDisconnected
        {
            add => _server.ClientDisconnected += value;
            remove => _server.ClientDisconnected -= value;
        }
        
        public MessageServer(NetworkServer server) : this(server, Encoding.UTF8)
        {
        }

        public MessageServer(NetworkServer server, Encoding encoding)
        {
            _server = server;
            _encoding = encoding;
        }

        public Message ReadMessage() => TryReadMessage(out var message) ? message : null;

        public bool TryReadMessage(out Message message)
        {
            using (var buffer = new MemoryStream())
            {
                foreach (var kvp in _server.Clients)
                {
                    var guid = kvp.Key;
                    var client = kvp.Value;

                    if (!client.Connected)
                        continue;
                    if (!_server.ReadMessage(guid, buffer))
                        continue;

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

        public bool WriteMessage(Guid guid, Message message)
        {
            using (var buffer = new MemoryStream())
            {
                using (var writer = new BinaryWriter(buffer, Encoding))
                {
                    var serializer = new BinarySerializer(writer);
                    message.Serialize(serializer);
                    if (_server.WriteMessage(guid, buffer))
                    {
                        Sent.Invoke(message);
                        return true;
                    }

                    return false;
                }
            }
        }

        public void WriteMessageRelay(Guid guid, Message message)
        {
            foreach (var key in _server.Clients.Keys)
            {
                if (key == guid)
                    continue;
                WriteMessage(key, message);
            }
        }

        public void WriteMessageAll(Message message)
        {
            foreach (var key in _server.Clients.Keys)
                WriteMessage(key, message);
        }

        public void WriteMessageMany(Message message, params Guid[] guids)
        {
            foreach (var key in guids)
                WriteMessage(key, message);
        }
    }
}