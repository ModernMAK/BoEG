using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace Framework.Core.Networking
{
    public class NetworkServer : NetworkStreamTransport, IDisposable
    {
        public NetworkServer(IPEndPoint endPoint) : this(new TcpListener(endPoint))
        {
        }

        public NetworkServer(TcpListener listener)
        {
            _listener = listener;
            _clients = new Dictionary<Guid, TcpClient>();
        }

        private readonly TcpListener _listener;
        private readonly Dictionary<Guid, TcpClient> _clients;
        protected TcpListener Listener => _listener;
        public IReadOnlyDictionary<Guid, TcpClient> Clients => _clients;

        public bool Pending() => _listener.Pending();


        public void Start()
        {
            _listener.Start();
            OnServerConnected();
        }

        public void Start(int maxRequests)
        {
            _listener.Start(maxRequests);
            OnServerConnected();
        }

        public void Stop()
        {
            _listener.Stop();
            DisconnectClients();
            _clients.Clear();
            OnServerDisconnected();
        }

        public TcpClient AcceptClient(out Guid guid)
        {
            var client = Listener.AcceptTcpClient();
            RegisterClient(client, out guid);
            OnClientConnected(guid);
            return client;
        }

        public bool TryAcceptClient(out TcpClient client, out Guid guid)
        {
            if (Pending())
            {
                client = AcceptClient(out guid);
                return true;
            }

            guid = Guid.Empty;
            client = default;
            return false;
        }

        public void RegisterClient(TcpClient client, out Guid guid)
        {
            guid = Guid.NewGuid();
            _clients.Add(guid, client);
        }


        public void DisconnectClients()
        {
            foreach (var kvp in Clients)
            {
                var guid = kvp.Key;
                var client = kvp.Value;
                DisconnectClient(guid, client);
            }
        }

        public event EventHandler<Guid> ClientConnected;
        public event EventHandler<Guid> ClientDisconnected;

        public event EventHandler<Tuple<Guid, Stream>> MessageReceived;
        public event EventHandler<Tuple<Guid, Stream>> MessageSent;

        public event EventHandler ServerStarted;
        public event EventHandler ServerStopped;


        public void ReadAllMessages()
        {
            foreach (var _ in ReadAndGetAllMessages())
            {
            }
        }

        public IEnumerable<MemoryStream> ReadAndGetAllMessages()
        {
            foreach (var kvp in Clients)
            {
                var guid = kvp.Key;
                var client = kvp.Value;
                if (!client.Connected)
                    continue;

                using (var memory = new MemoryStream(MessageBufferSize))
                {
                    ReadMessage(guid, client, memory);
                    yield return memory;
                }
            }
        }

        private void DisconnectClient(Guid guid, TcpClient client)
        {
            Debug.Log("Disconnect Client - Closed & Disposed");
            _clients.Remove(guid);
            client.Close();
            client.Dispose();
            OnClientDisconnected(guid);
        }

        public bool ReadMessage(Guid guid, MemoryStream stream) => ReadMessage(guid, Clients[guid], stream);

        private bool ReadMessage(Guid guid, TcpClient client, MemoryStream stream)
        {
            if (!client.Connected)
                return false;
            // using (var net = client.GetStream())
            // {
            if (!TryReadMessage(client, stream, out var read))
                return false;
            if (read)
            {
                OnMessageReceived(new Tuple<Guid, Stream>(guid, new ReadOnlyStream(stream)));
            }

            return read;

            // }
        }

        public void WriteMessageRelay(Guid sender, Stream stream)
        {
            var pos = stream.Position;
            foreach (var kvp in Clients)
            {
                if (kvp.Key == sender)
                    continue;
                stream.Seek(pos, SeekOrigin.Begin);
                WriteMessage(kvp.Key, kvp.Value, stream);
            }
        }

        public void WriteMessageToAll(Stream stream)
        {
            var pos = stream.Position;
            foreach (var kvp in Clients)
            {
                stream.Seek(pos, SeekOrigin.Begin);
                WriteMessage(kvp.Key, kvp.Value, stream);
            }
        }

        public void WriteMessageToMany(Stream stream, params Guid[] guids)
        {
            var pos = stream.Position;
            foreach (var guid in guids)
            {
                stream.Seek(pos, SeekOrigin.Begin);
                WriteMessage(guid, stream);
            }
        }

        public bool WriteMessage(Guid guid, Stream stream) => WriteMessage(guid, Clients[guid], stream);

        private bool WriteMessage(Guid guid, TcpClient client, Stream stream)
        {
            // using (var netStream = client.GetStream())
            // {
            if (WriteMessage(client.GetStream(), stream))
            {
                OnMessageSent(new Tuple<Guid, Stream>(guid, new ReadOnlyStream(stream)));
                return true;
            }

            return false;

            // }
        }


        protected virtual void OnServerConnected()
        {
            ServerStarted?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnServerDisconnected()
        {
            ServerStopped?.Invoke(this, EventArgs.Empty);
        }


        protected virtual void OnClientDisconnected(Guid e)
        {
            ClientDisconnected?.Invoke(this, e);
        }

        protected virtual void OnClientConnected(Guid e)
        {
            ClientConnected?.Invoke(this, e);
        }


        protected virtual void OnMessageReceived(Tuple<Guid, Stream> e)
        {
            MessageReceived?.Invoke(this, e);
        }

        protected virtual void OnMessageSent(Tuple<Guid, Stream> e)
        {
            MessageSent?.Invoke(this, e);
        }

        public void Dispose()
        {
            Debug.Log("Disposed NetworkServer");
            Stop();
        }

        public void DropDisconnectedClients()
        {
            Queue<KeyValuePair<Guid, TcpClient>> _toDrop = new Queue<KeyValuePair<Guid, TcpClient>>();
            foreach (var kvp in Clients)
            {
                if (kvp.Value.Connected)
                    continue;
                _toDrop.Enqueue(kvp);
            }

            while (_toDrop.Count > 0)
            {
                var kvp = _toDrop.Dequeue();
                DisconnectClient(kvp.Key, kvp.Value);
            }
        }
    }
}