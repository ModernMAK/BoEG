using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Framework.Core.Networking
{
    public class NetworkServer : IDisposable
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
        public TcpListener Listener => _listener;
        public IReadOnlyDictionary<Guid, TcpClient> Clients => _clients;

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

        public const int MessageBufferSize = 1024;
        private static readonly byte[] MessageBuffer = new byte[MessageBufferSize];

        public static void ReadMessage(NetworkStream netStream, MemoryStream memStream)
        {
            while (netStream.DataAvailable)
            {
                var bytesRead = netStream.Read(MessageBuffer, 0, MessageBufferSize);
                memStream.Write(MessageBuffer, 0, bytesRead);
            }
        }

        public static bool TryReadMessage(NetworkStream netStream, MemoryStream memStream)
        {
            var read = netStream.DataAvailable;
            ReadMessage(netStream, memStream);
            return read;
        }

        public static void WriteMessage(NetworkStream netStream, MemoryStream memStream)
        {
            while (true)
            {
                var bytesRead = memStream.Read(MessageBuffer, 0, MessageBufferSize);
                if (bytesRead > 0)
                    netStream.Write(MessageBuffer, 0, bytesRead);
                else
                    return;
            }
        }


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
                {
                    DisconnectClient(guid, client);
                }

                using (var memory = new MemoryStream(MessageBufferSize))
                {
                    ReadMessage(guid, client, memory);
                    yield return memory;
                }
            }
        }

        private void DisconnectClient(Guid guid, TcpClient client)
        {
            client.Close();
            client.Dispose();
            OnClientDisconnected(new Tuple<NetworkServer, Guid, TcpClient>(this, guid, client));
        }

        public bool ReadMessage(Guid guid, MemoryStream stream) => ReadMessage(guid, Clients[guid], stream);

        private bool ReadMessage(Guid guid, TcpClient client, MemoryStream stream)
        {
            if (!client.Connected)
                return false;
            using (var net = client.GetStream())
            {
                if (!TryReadMessage(net, stream))
                    return false;
                OnMessageRecieved(new Tuple<NetworkServer, Guid, TcpClient, MemoryStream>(this, guid, client, stream));
                return true;
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
            using (var netStream = client.GetStream())
            {
                WriteMessage(netStream, stream);
                OnMessageSent(new Tuple<NetworkServer, Guid, TcpClient, MemoryStream>(this, guid, client, stream));
            }
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
            Stop();
        }
    }
}