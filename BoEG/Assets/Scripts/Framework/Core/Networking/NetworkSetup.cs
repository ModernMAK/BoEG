using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Framework.Core.Networking
{
    public class NetworkSetup : MonoBehaviour
    {
        [SerializeField] private InputField _input;
        // public static IPAddress[] GetLanIPs() => Dns.GetHostAddresses(Dns.GetHostName());
        // public static IPAddress[] GetMyIPs() => Dns.GetHostAddresses("localhost");
        //
        // public static IEnumerable<IPEndPoint> ScanPorts(Socket socket, IEnumerable<IPAddress> addresses,
        //     params int[] ports)
        // {
        //     foreach (var address in addresses)
        //     {
        //         foreach (var port in ports)f.c
        //         {
        //             bool success;
        //             try
        //             {
        //                 socket.Connect(address, port);
        //                 success = socket.Connected;
        //                 socket.Disconnect(true);
        //             }
        //             catch (SocketException exception)
        //             {
        //                 success = false;
        //             }
        //
        //             //Cant yield in try-catch statement
        //             if (success)
        //
        //                 yield return new IPEndPoint(address, port);
        //         }
        //     }
        // }


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

        // [MenuItem("Networking/Scan Ips & Ports")]
        // public static void DebugScanIpAndPorts()
        // {
        //     var lanIps = GetLanIPs();
        //     var selfIps = GetMyIPs();
        //     var set = new HashSet<IPAddress>();
        //     foreach (var ip in lanIps)
        //         set.Add(ip);
        //     foreach (var ip in selfIps)
        //         set.Add(ip);
        //     var ips = (IEnumerable<IPAddress>)set;
        //     
        //     var ports = new[] {DefaultPort};
        //     using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
        //     {
        //         var ipsAndPorts = ScanPorts(socket, ips, ports);
        //         var counter = 0;
        //         var log = "";
        //         log += "IPs ===\n";
        //
        //         foreach (var ip in ips)
        //         {
        //             counter++;
        //             log += $"[{counter}]\t{ip}\n";
        //         }
        //
        //         if (counter == 0)
        //             log += $"[-1]\tNo IPs";
        //         log += "IP:Ports ===\n";
        //         counter = 0;
        //         foreach (var endPoint in ipsAndPorts)
        //         {
        //             var ip = endPoint.Address;
        //             var port = endPoint.Port;
        //             counter++;
        //             log += $"[{counter}]\t{ip}\t{port}\n";
        //         }
        //
        //         if (counter == 0)
        //             log += $"[_]\tNo IPs on Ports";
        //
        //         Debug.Log(log);
        //     }
        // }

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

        private void ClientOnMessageSent(object sender, Stream e)
        {
            
            using (var reader = new StreamReader(new NonClosingStream(e), Encoding.ASCII))
            {
                e.Seek(0, SeekOrigin.Begin);
                var msg = reader.ReadLine();
                _text.text += "RECV:" + msg + "\n";
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

        private void ClientOnMessageReceived(object sender, Stream stream)
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
            _server.MessageRecieved += ServerOnMessageRecieved;
            _server.MessageSent += ServerOnMessageSent;
            if (maxRequests <= 0)
                _server.Start();
            else
                _server.Start(maxRequests);
        }

        private void ServerOnClientConnected(object sender, Tuple<NetworkServer, Guid, TcpClient> e)
        {
            var msg = ConnectedMessage($"Client [{e.Item2.ToString()}]", true, true, e.Item3.Client.RemoteEndPoint);
            _text.text += msg;
        }

        private void ServerOnClientDisconnected(object sender, Tuple<NetworkServer, Guid, TcpClient> e)
        {
            var msg = ConnectedMessage($"Client [{e.Item2.ToString()}]", false, true, null);
            if (_text)
                _text.text += msg;
        }

        private void ServerOnMessageRecieved(object sender, Tuple<NetworkServer, Guid, TcpClient, MemoryStream> e)
        {
            e.Item4.Seek(0, SeekOrigin.Begin);
            var stream = e.Item4;
            using (var reader = new StreamReader(new NonClosingStream(stream), Encoding.ASCII))
            {
                stream.Seek(0, SeekOrigin.Begin);
                var msg = reader.ReadLine();
                _text.text += "RECV:" + msg + "\n";
            }

            stream.Seek(0, SeekOrigin.Begin);
            _server.WriteMessageRelay(e.Item2, stream);
        }

        private void ServerOnMessageSent(object sender, Tuple<NetworkServer, Guid, TcpClient, MemoryStream> e)
        {
            var stream = e.Item4;
            using (var reader = new StreamReader(new NonClosingStream(stream), Encoding.ASCII))
            {
                stream.Seek(0, SeekOrigin.Begin);
                var msg = reader.ReadLine();
                _text.text += "SENT:" + msg + "\n";
            }
            stream.Seek(0, SeekOrigin.Begin);
        }

        private void ServerOnServerStarted(object sender, Tuple<NetworkServer> e)
        {
            if (_text == null)
                return;
            // var socketInfo = (IPEndPoint) e.Item1.Listener.LocalEndpoint;
            _text.text += $"Started Server\n";
        }

        private void ServerOnSeverStopped(object sender, Tuple<NetworkServer> e)
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
                Debug.Log("Not online!");
                return;
            }

            var msg = _input.text;
            _input.text = "";
            if (msg == "")
            {
                _text.text += "[LOG]: No Text To Send\n";
                Debug.Log("No Text To Send");
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

            // else if (IsClient)
            // {
            //     if (_client.Connected)
            //     {
            //         var ep = ((IPEndPoint) _client.Client.RemoteEndPoint);
            //         _text.text += $"Client Disconnected @ {ep.Address} : {ep.Port}\n";
            //         _client.Dispose();
            //         _client = null;
            //     }
            // }
        }
    }
}