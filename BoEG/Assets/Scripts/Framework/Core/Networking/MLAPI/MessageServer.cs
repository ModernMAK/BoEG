using System;
using System.IO;
using System.Net;
using System.Text;
using MobaGame.Framework.Core.Networking.LLAPI;
using MobaGame.Framework.Core.Serialization;

namespace MobaGame.Framework.Core.Networking.MLAPI
{
    public class MessageServer
    {
        public MessageEventList Sent { get; }
        public MessageSenderEventList SentTo { get; }
        public MessageEventList Received { get; }
        public MessageSenderEventList ReceivedFrom { get; }

        private readonly NetworkServer _server;
        private Encoding _encoding;

        public Encoding Encoding
        {
            get => _encoding;
            set => _encoding = value;
        }
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
            Sent = new MessageEventList();
            SentTo = new MessageSenderEventList();
            Received = new MessageEventList();
            ReceivedFrom = new MessageSenderEventList();
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

                    buffer.Seek(0, SeekOrigin.Begin);
                    using (var reader = new BinaryReader(buffer, Encoding))
                    {
                        var deserializer = new BinaryDeserializer(reader);
                        message = new Message();
                        message.Deserialize(deserializer);
                        ReceivedFrom.Invoke(new MessageSenderEventArgs(guid, message));
                        Received.Invoke(message);
                        return true;
                    }
                }
            }

            message = default;
            return false;
        }

        private bool InternalWriteMessage(Guid guid, Message message)
        {
            using (var buffer = new MemoryStream())
            {
                using (var writer = new BinaryWriter(buffer, Encoding))
                {
                    var serializer = new BinarySerializer(writer);
                    message.Serialize(serializer);
                    buffer.Seek(0, SeekOrigin.Begin);
                    if (_server.WriteMessage(guid, buffer))
                    {
                        Sent.Invoke(message);
                        return true;
                    }

                    return false;
                }
            }
        }

        public bool WriteMessage(Guid guid, Message message)
        {
            if (InternalWriteMessage(guid, message))
            {
                Sent.Invoke(message);
                SentTo.Invoke(new MessageSenderEventArgs(guid, message));
                return true;
            }

            return false;
        }

        public void WriteMessageRelay(Guid guid, Message message)
        {
            bool sent = false;
            foreach (var key in _server.Clients.Keys)
            {
                if (key == guid)
                    continue;

                if (InternalWriteMessage(key, message))
                {
                    SentTo.Invoke(new MessageSenderEventArgs(key, message));
                    sent = true;
                }
            }

            if (sent)
                Sent.Invoke(message);
        }

        public void WriteMessageAll(Message message)
        {
            bool sent = false;
            foreach (var key in _server.Clients.Keys)
            {
                if (InternalWriteMessage(key, message))
                {
                    SentTo.Invoke(new MessageSenderEventArgs(key, message));
                    sent = true;
                }
            }

            if (sent)
                Sent.Invoke(message);
        }

        public void WriteMessageMany(Message message, params Guid[] guids)
        {
            bool sent = false;
            foreach (var key in guids)
            {
                if (InternalWriteMessage(key, message))
                {
                    SentTo.Invoke(new MessageSenderEventArgs(key, message));
                    sent = true;
                }
            }

            if (sent)
                Sent.Invoke(message);
        }

        public void Rebind(IPEndPoint endPoint) => InternalServer.Rebind(endPoint);
        
        public void Start() => _server.Start();
        public void Start(int connections) => _server.Start(connections);

        public void ReadAllMessages()
        {
            while (TryReadMessage(out _))
            {
            }
        }

        public void Stop() => InternalServer.Stop();
    }
}