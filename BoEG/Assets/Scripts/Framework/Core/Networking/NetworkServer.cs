using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace Framework.Core.Networking
{
    public class NetworkServer : NetworkTransport, IDisposable
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
        protected IReadOnlyDictionary<Guid, TcpClient> Clients => _clients;

        public bool Pending() => _listener.Pending();


        public void Start()
        {
            _listener.Start();
            OnServerConnected(new Tuple<NetworkServer>(this));
        }

        public void Start(int maxRequests)
        {
            _listener.Start(maxRequests);
            OnServerConnected(new Tuple<NetworkServer>(this));
        }

        public void Stop()
        {
            _listener.Stop();
            DisconnectClients();
            _clients.Clear();
            OnServerDisconnected(new Tuple<NetworkServer>(this));
        }

        public TcpClient AcceptClient(out Guid guid)
        {
            var client = Listener.AcceptTcpClient();
            RegisterClient(client, out guid);
            OnClientConnected(new Tuple<NetworkServer, Guid, TcpClient>(this, guid, client));
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

        public event EventHandler<Tuple<NetworkServer, Guid, TcpClient>> ClientConnected;
        public event EventHandler<Tuple<NetworkServer, Guid, TcpClient>> ClientDisconnected;

        public event EventHandler<Tuple<NetworkServer, Guid, TcpClient, MemoryStream>> MessageRecieved;
        public event EventHandler<Tuple<NetworkServer, Guid, TcpClient, MemoryStream>> MessageSent;

        public event EventHandler<Tuple<NetworkServer>> ServerStarted;
        public event EventHandler<Tuple<NetworkServer>> ServerStopped;


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
            client.Close();
            client.Dispose();
            OnClientDisconnected(new Tuple<NetworkServer, Guid, TcpClient>(this, guid, client));
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
                OnMessageRecieved(new Tuple<NetworkServer, Guid, TcpClient, MemoryStream>(this, guid, client, stream));
            }

            return read;

            // }
        }

        public void WriteMessageRelay(Guid sender, MemoryStream stream)
        {
            var pos = stream.Position;
            foreach (var kvp in Clients)
            {
                if(kvp.Key == sender)
                    continue;
                stream.Seek(pos, SeekOrigin.Begin);
                WriteMessage(kvp.Key, kvp.Value, stream);
            }
        }
        public void WriteMessageToAll(MemoryStream stream)
        {
            var pos = stream.Position;
            foreach (var kvp in Clients)
            {
                stream.Seek(pos, SeekOrigin.Begin);
                WriteMessage(kvp.Key, kvp.Value, stream);
            }
        }

        public void WriteMessageToMany(MemoryStream stream, params Guid[] guids)
        {
            var pos = stream.Position;
            foreach (var guid in guids)
            {
                stream.Seek(pos, SeekOrigin.Begin);
                WriteMessage(guid, stream);
            }
        }

        public void WriteMessage(Guid guid, MemoryStream stream) => WriteMessage(guid, Clients[guid], stream);

        private void WriteMessage(Guid guid, TcpClient client, MemoryStream stream)
        {
            // using (var netStream = client.GetStream())
            // {
            if (WriteMessage(client.GetStream(), stream))
                OnMessageSent(new Tuple<NetworkServer, Guid, TcpClient, MemoryStream>(this, guid, client, stream));
            // }
        }


        protected virtual void OnServerConnected(Tuple<NetworkServer> e)
        {
            ServerStarted?.Invoke(this, e);
        }

        protected virtual void OnServerDisconnected(Tuple<NetworkServer> e)
        {
            ServerStopped?.Invoke(this, e);
        }


        protected virtual void OnClientDisconnected(Tuple<NetworkServer, Guid, TcpClient> e)
        {
            ClientDisconnected?.Invoke(this, e);
        }

        protected virtual void OnClientConnected(Tuple<NetworkServer, Guid, TcpClient> e)
        {
            ClientConnected?.Invoke(this, e);
        }


        protected virtual void OnMessageRecieved(Tuple<NetworkServer, Guid, TcpClient, MemoryStream> e)
        {
            MessageRecieved?.Invoke(this, e);
        }

        protected virtual void OnMessageSent(Tuple<NetworkServer, Guid, TcpClient, MemoryStream> e)
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
                _clients.Remove(kvp.Key);
                var c = kvp.Value;
                c.Close();
                c.Dispose();
            }
        }
    }
}