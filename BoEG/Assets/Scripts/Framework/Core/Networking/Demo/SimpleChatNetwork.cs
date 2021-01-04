using System;
using System.IO;
using System.Net;
using System.Text;
using MobaGame.Framework.Core.Networking.LLAPI;
using MobaGame.Framework.Core.Networking.Stream;
using UnityEngine;
using UnityEngine.UI;

namespace MobaGame.Framework.Core.Networking.Demo
{
    public class SimpleChatNetwork : MonoBehaviour
    {
#pragma warning disable 0649

        [SerializeField] private InputField _input;
#pragma warning restore 0649

        public const string DefaultHost = "127.0.0.1";
        public const int DefaultPort = 8564;

        public static IPEndPoint DefaultEndPoint
        {
            get
            {
                var address = IPAddress.Parse(DefaultHost);
                return new IPEndPoint(address, DefaultPort);
            }
        }


        private NetworkServer _server;
        private NetworkClient _client;

        public bool IsClient => _client != null;
        public bool IsServer => _server != null;
        public bool IsOnline => IsClient || IsServer;

        public Text _text;


        public void StartClient() => StartClient(DefaultEndPoint);

        public void StartClient(IPEndPoint serverInfo)
        {
            if (IsOnline)
            {
                _text.text += $"Cannot Start Client While Online\n";
                return;
            }

            var ep = serverInfo;
            _text.text += $"Started Client @ {ep.Address} : {ep.Port}\n";
            _client = new NetworkClient();
            _client.ClientConnected += ClientOnClientConnected;
            _client.ClientDisconnected += ClientOnClientDisconnected;
            _client.MessageReceived += ClientOnMessageReceived;
            _client.MessageSent += ClientOnMessageSent;
            if (!_client.TryConnect(serverInfo))
            {
                _text.text += "Killed Client";
                _client.Close();
                _client = null;
            }
        }

        private void ClientOnMessageSent(object sender, System.IO.Stream e)
        {
            
            using (var reader = new StreamReader(new NonClosingStream(e), Encoding.ASCII))
            {
                e.Seek(0, SeekOrigin.Begin);
                var msg = reader.ReadLine();
                _text.text += "SENT:" + msg + "\n";
            }

        }

        public string ConnectedMessage(string name, bool connecting, bool from, EndPoint endPoint)
        {
            var ipep = (IPEndPoint) endPoint;
            var fromStr = from ? "FROM" : "TO";
            var connectingStr = connecting ? "Connected" : "Disconnected";
            if (ipep == null)
                return $"{name} {connectingStr}\n";
            else
                return $"{name} {connectingStr} {fromStr} {ipep.Address} : {ipep.Port}\n";
        }

        private void ClientOnClientConnected(object sender, EventArgs eventArgs)
        {
            var client = (NetworkClient) sender;
            var msg = ConnectedMessage("Client", true, true, client.RemoteEndPoint);
            _text.text += msg;
        }

        private void ClientOnClientDisconnected(object sender, EventArgs eventArgs)
        {
            var msg = ConnectedMessage("Client", false, false, null);
            _text.text += msg;
        }

        private void ClientOnMessageReceived(object sender, System.IO.Stream stream)
        {
            using (var reader = new StreamReader(new NonClosingStream(stream), Encoding.ASCII))
            {
                stream.Seek(0, SeekOrigin.Begin);
                var msg = reader.ReadLine();
                _text.text += "RECV:" + msg + "\n";
            }
        }

        public void StartServer() => StartServer(DefaultEndPoint);

        public void StartServer(IPEndPoint socketInfo, int maxRequests = 0)
        {
            if (IsOnline)
            {
                _text.text += $"Cannot Start Server While Online\n";
                return;
            }

            _server = new NetworkServer(socketInfo);
            _server.ServerStarted += ServerOnServerStarted;
            _server.ServerStopped += ServerOnSeverStopped;
            _server.ClientConnected += ServerOnClientConnected;
            _server.ClientDisconnected += ServerOnClientDisconnected;
            _server.MessageReceived += ServerOnMessageReceived;
            _server.MessageSent += ServerOnMessageSent;
            if (maxRequests <= 0)
                _server.Start();
            else
                _server.Start(maxRequests);
        }

        private void ServerOnClientConnected(object sender, Guid guid)
        {
            var msg = ConnectedMessage($"Client [{guid}]", true, true, null);
            _text.text += msg;
        }

        private void ServerOnClientDisconnected(object sender, Guid guid)
        {
            var msg = ConnectedMessage($"Client [{guid}]", false, true, null);
            if (_text)
                _text.text += msg;
        }

        private void ServerOnMessageReceived(object sender, Tuple<Guid, System.IO.Stream> tuple)
        {
            var guid = tuple.Item1;
            var stream = tuple.Item2;
            
            stream.Seek(0, SeekOrigin.Begin);
            using (var reader = new StreamReader(new NonClosingStream(stream), Encoding.ASCII))
            {
                stream.Seek(0, SeekOrigin.Begin);
                var msg = reader.ReadLine();
                _text.text += "RECV:" + msg + "\n";
            }

            stream.Seek(0, SeekOrigin.Begin);
            _server.WriteMessageRelay(guid, stream);
        }

        private void ServerOnMessageSent(object sender, Tuple<Guid, System.IO.Stream> tuple)
        {
            var guid = tuple.Item1;
            var stream = tuple.Item2;
            using (var reader = new StreamReader(new NonClosingStream(stream), Encoding.ASCII))
            {
                stream.Seek(0, SeekOrigin.Begin);
                var msg = reader.ReadLine();
                _text.text += "SENT:" + msg + "\n";
            }
            stream.Seek(0, SeekOrigin.Begin);
        }

        private void ServerOnServerStarted(object sender, EventArgs eventArgs)
        {
            if (_text == null)
                return;
            // var socketInfo = (IPEndPoint) e.Item1.Listener.LocalEndpoint;
            _text.text += $"Started Server\n";
        }

        private void ServerOnSeverStopped(object sender, EventArgs eventArgs)
        {
            if (_text == null)
                return;
            // var socketInfo = (IPEndPoint) _server.Listener.LocalEndpoint;
            _text.text += $"Killed Server\n";
        }

        public void Stop()
        {
            if (_server != null)
            {
                _server.Stop();
                _server = null;
            }

            if (_client != null)
            {
                _client.Close();
                _client = null;
            }
        }


        public void SendTextMessage()
        {
            if (!IsOnline)
            {
                _text.text += "[LOG]: Not Online!\n";
                UnityEngine.Debug.Log("Not online!");
                return;
            }

            var msg = _input.text;
            _input.text = "";
            if (msg == "")
            {
                _text.text += "[LOG]: No Text To Send\n";
                UnityEngine.Debug.Log("No Text To Send");
                return;
            }

            using (var memory = new MemoryStream())
            {
                using (var writer = new StreamWriter(new NonClosingStream(memory), Encoding.ASCII))
                {
                    writer.WriteLine(msg);
                    writer.Flush();
                    memory.Seek(0, SeekOrigin.Begin);
                    if (IsClient)
                        _client.WriteMessage(memory);
                    else if (IsServer)
                        _server.WriteMessageToAll(memory);
                }
            }
        }

        private void OnDestroy()
        {
            if (_server != null)
                _server.Stop();
            if (_client != null)
                _client.Close();
        }

        private void Update()
        {
            if (IsServer)
            {
                _server.TryAcceptClient(out _, out _);
                _server.ReadAllMessages();
                _server.DropDisconnectedClients();
            }

            else if (IsClient)
            {
                _client.ReadAllMessages();
                if(!_client.CheckServer())
                    Stop();
            }
        }
    }
}