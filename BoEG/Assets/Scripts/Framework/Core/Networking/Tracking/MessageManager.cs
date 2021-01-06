using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using MobaGame.Framework.Core.Networking.LLAPI;
using MobaGame.Framework.Core.Networking.MLAPI;
using UnityEngine;

namespace MobaGame.Framework.Core.Networking.Tracking
{
    public class MessageManager : MonoBehaviour
    {
        public enum ServerMode
        {
            /// <summary>
            /// The Server will act as an authoritative server relaying messages to clients
            /// The Server is allowed to 'own' all state on the server, even if it belongs to a client 
            /// </summary>
            RemoteHost,

            /// <summary>
            /// The server will act as an authoritative server relaying messages to clients
            /// BUT the server also hosts a client connection
            /// The Server is allowed to 'own' all state on the server, even if it belongs to a client
            /// The 'Client' does not 'own' all state on the server, (unless it is using the server to gain access)
            /// </summary>
            LocalHost,
        }

        private static MessageManager _instance;
        private const string SingletonName = NetworkSingletonNames.NetManager;

        private MessageClient _client;
        private MessageServer _server;
        private bool _isClient;
        private bool _isServer;
        private ServerMode _mode = ServerMode.RemoteHost;

        public MessageClient MessageClient => _client;

        public MessageServer MessageServer => _server;

        public static MessageManager Instance =>
            SingletonUtility.GetInstance(ref _instance, SingletonName, NetworkSingletonNames.SingletonGroup);

        protected void Awake()
        {
            if (_instance == null)
                _instance = this;
            else
                UnityEngine.Debug.Log($"Multiple {SingletonName}!");
            _isClient = _isServer = false;
            _client = new MessageClient();
            _server = new MessageServer(
                new NetworkServer(new TcpListener(IPAddress.Any, 8000))); //Dummy information, this will be rebound
        }

        public bool IsServer => _isServer;
        public bool IsServerOnline => IsServer && MessageServer.InternalServer.Online;
        public bool IsClient => _isClient;
        public bool IsClientOnline => IsClient && MessageClient.InternalClient.Online;
        public bool IsOnline => IsServerOnline || IsClientOnline;

        public bool IsLocalClient => _mode switch
        {
            ServerMode.RemoteHost => IsClient,
            ServerMode.LocalHost => IsClient || IsServer,
            _ => throw new ArgumentOutOfRangeException()
        };
        // SERVER ==========

        public void InitializeServer(IPEndPoint endPoint) => InitializeServer(endPoint, ServerMode.RemoteHost);

        public void InitializeServer(IPEndPoint endPoint, ServerMode mode) =>
            InitializeServer(endPoint, mode, Encoding.UTF8);

        public void InitializeServer(IPEndPoint endPoint, ServerMode mode, Encoding encoding)
        {
            if (IsServer)
            {
                UnityEngine.Debug.Log("Cannot Initialize Server ~ 'Server Is Already Initialized'");
                return;
            }
            else if (IsClient)
            {
                UnityEngine.Debug.Log("Cannot Initialize Server ~ 'Client Is Already Initialized'");
                return;
            }

            _mode = mode;
            _server.Encoding = encoding;
            _server.Rebind(endPoint);
            _isServer = true;
        }

        public void StartServer(int maxConnections)
        {
            if (IsServer)
                _server.Start(maxConnections);
        }

        public void StartServer()
        {
            if (IsServer)
                _server.Start();
        }

        public void StopServer()
        {
            if (IsServer)
                _server.Stop();
        }

        public void KillServer()
        {
            if (!IsServer)
            {
                UnityEngine.Debug.Log("Cannot Kill Server ~ 'Server Is Not Initialized'");
                return;
            }
            else if (IsServerOnline)
            {
                MessageServer.Stop();
                UnityEngine.Debug.Log("Server Stopped ~ 'Server Was Killed'");
            }

            StopServer();
            _isServer = false;
        }

        // CLIENT ==========

        public void InitializeClient() => InitializeClient(Encoding.UTF8);

        public void InitializeClient(Encoding encoding)
        {
            if (IsServer)
                UnityEngine.Debug.Log("Cannot Initialize Client ~ 'Server Is Already Online'");
            else if (IsClient)
                UnityEngine.Debug.Log("Cannot Initialize Client ~ 'Client Is Already Online'");

            _client.Encoding = encoding;
            _isClient = true;
        }

        public void KillClient()
        {
            if (!IsClient)
            {
                UnityEngine.Debug.Log("Cannot Kill Client ~ 'Client Is Not Initialized'");
                return;
            }
            else if (IsClientOnline)
            {
                MessageClient.Close();
                UnityEngine.Debug.Log("Client Closed ~ 'Client Was Killed'");
            }

            _isClient = false;
        }

        public void ReadAllMessages()
        {
            if (IsClientOnline)
                MessageClient.ReadAllMessages();
            else if (IsServerOnline)
                MessageServer.ReadAllMessages();
        }

        public void TryAcceptClients()
        {
            if (IsServerOnline)
                MessageServer.InternalServer.TryAcceptClient(out _, out _);
        }

        public void CheckConnections()
        {
            if (IsServerOnline)
                MessageServer.InternalServer.DropDisconnectedClients();
            else if (IsClientOnline)
                MessageClient.InternalClient.CheckServer();
        }

        public void UpdateNetwork()
        {
            if (IsServerOnline)
            {
                MessageServer.InternalServer.DropDisconnectedClients();
                MessageServer.InternalServer.TryAcceptClient(out _, out _);
                MessageServer.ReadAllMessages();
            }
            else if (IsClientOnline)
            {
                MessageClient.InternalClient.CheckServer();
                MessageClient.ReadAllMessages();
            }
        }

        private void OnDestroy()
        {
            if (IsClient)
                KillClient();
            if (IsServer)
                KillServer();
        }

        // =======
        // A note here for later; this still exists, despite being obsolete
        // This was removed in 2018, and no longer functions
        // private void OnServerInitialized() {}
        // private void OnConnectedToServer() {}
        // ===
        // I mostly wanted to mention this because while rewriting mine, i keep finding remnants of the old stuff and go
        // 'wait is that still there?' I can safely say; its there, but just dont use it.
        // =======
    }
}