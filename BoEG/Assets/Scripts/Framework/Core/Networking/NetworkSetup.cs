using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

namespace Framework.Core.Networking
{
    public class NetworkSetup : MonoBehaviour
    {
        [SerializeField] private Text _input;
        // public static IPAddress[] GetLanIPs() => Dns.GetHostAddresses(Dns.GetHostName());
        // public static IPAddress[] GetMyIPs() => Dns.GetHostAddresses("localhost");
        //
        // public static IEnumerable<IPEndPoint> ScanPorts(Socket socket, IEnumerable<IPAddress> addresses,
        //     params int[] ports)
        // {
        //     foreach (var address in addresses)
        //     {
        //         foreach (var port in ports)
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
        public static IPEndPoint DefaultEndPoint => new IPEndPoint(IPAddress.Parse(DefaultHost), DefaultPort);

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
            _client.MessageSent += ClientOnMessageReceived;
            try
            {
                _client.Connect(serverInfo);
            }
            catch (SocketException exception)
            {
                _text.text += $"Client Failed To Connect @ {serverInfo.Address} : {serverInfo.Port}\n";
                _text.text += $"\t[EX]: {exception.Message}\n";
                _client.Close();
                _client = null;
            }
        }

        private void ClientOnClientConnected(object sender, Tuple<NetworkClient> e)
        {
            var serverInfo = (IPEndPoint) e.Item1.Client.Client.RemoteEndPoint;
            _text.text += $"Client Connected @ {serverInfo.Address} : {serverInfo.Port}\n";
        }

        private void ClientOnClientDisconnected(object sender, Tuple<NetworkClient> e)
        {
            var serverInfo = (IPEndPoint) e.Item1.Client.Client.RemoteEndPoint;
            _text.text += $"Client Disconnected @ {serverInfo.Address} : {serverInfo.Port}\n";
        }

        private void ClientOnMessageReceived(object sender, Tuple<NetworkClient, MemoryStream> e)
        {
            using (var reader = new StreamReader(e.Item2))
                _text.text += $"{reader.ReadToEnd()}\n";
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
            _server.MessageRecieved += ServerOnMessageRecieved;
            if (maxRequests <= 0)
                _server.Start();
            else
                _server.Start(maxRequests);
        }

        private void ServerOnMessageRecieved(object sender, Tuple<NetworkServer, Guid, TcpClient, MemoryStream> e)
        {
            e.Item4.Seek(0, SeekOrigin.Begin);
            _server.WriteMessageToAll(e.Item4);
        }

        private void ServerOnServerStarted(object sender, Tuple<NetworkServer> e)
        {
            if (_text == null)
                return;
            var socketInfo = (IPEndPoint) e.Item1.Listener.LocalEndpoint;
            _text.text += $"Started Server @ {socketInfo.Address} : {socketInfo.Port}\n";
        }

        private void ServerOnSeverStopped(object sender, Tuple<NetworkServer> e)
        {
            if (_text == null)
                return;
            var socketInfo = (IPEndPoint) _server.Listener.LocalEndpoint;
            _text.text += $"Killed Server @ {socketInfo.Address} : {socketInfo.Port}\n";
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
            if(_input.text == "")
                return;
            
            if (IsClient)
            {
                using(var memory = new MemoryStream())
                using (var writer = new StreamWriter(memory))
                {
                    writer.WriteLine(_input.text);
                    _input.text = "";
                    _client.WriteMessage(memory);
                }
            }

            else if (IsServer)
            {
                
                using(var memory = new MemoryStream())
                using (var writer = new StreamWriter(memory))
                {
                    writer.WriteLine(_input.text);
                    _input.text = "";
                    _server.WriteMessageToAll(memory);
                }
            }
        }


        private void RelayMessageToClients() => SendMessageToClients(null);

        private void SendMessageToClients(MemoryStream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            _server.WriteMessageToAll(stream);
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
            }
            else if (IsClient)
            {
                _client.ReadMessages();
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