using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;


// public class TcpServer
// {
//     private readonly TcpListener _listener;
//     public TcpListener Listener => _listener;
//     public TcpServer(TcpListener listener)
//     {
//         _listener = listener;
//     }
// }

public class NetworkSetup : MonoBehaviour
{
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

    private TcpListener _server;
    private TcpClient _client;
    private List<TcpClient> _serverClients;

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
        _client = new TcpClient();
        try
        {
            _client.Connect(serverInfo);
            _text.text += $"Client Connected @ {serverInfo.Address} : {serverInfo.Port}\n";
        }
        catch (SocketException exception)
        {
            _text.text += $"Client Failed To Connect @ {serverInfo.Address} : {serverInfo.Port}\n";
            _text.text += $"\t[EX]: {exception.Message}\n";
            _client.Close();
            _client = null;
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

        _server = new TcpListener(socketInfo);
        if (maxRequests <= 0)
            _server.Start();
        else
            _server.Start(maxRequests);
        _text.text += $"Started Server @ {socketInfo.Address} : {socketInfo.Port}\n";
    }

    public void Stop()
    {
        if (_server != null)
        {
            var socketInfo = (IPEndPoint) _server.LocalEndpoint;
            _text.text += $"Killed Server @ {socketInfo.Address} : {socketInfo.Port}\n";
            _server.Stop();
            _server = null;
        }

        if (_client != null)
        {
            var socketInfo = (IPEndPoint) _client.Client.RemoteEndPoint;
            _text.text += $"Killed Client @ {socketInfo.Address} : {socketInfo.Port}\n";
            _client.Close();
            _client = null;
        }
    }

    private void Awake()
    {
        _serverClients = new List<TcpClient>();
    }

    private void OnDestroy()
    {
        if (_server != null)
        {
            foreach (var client in _serverClients)
            {
                if(client != null)
                    client.Close();
            }
            _serverClients.Clear();
            _server.Stop();
        }

        if (_client != null)
            _client.Close();
    }

    private void Update()
    {
        if (IsServer)
        {
            if (_server.Pending())
            {
                var client = _server.AcceptTcpClient();
                _serverClients.Add(client);
                var ep = ((IPEndPoint) client.Client.RemoteEndPoint);
                _text.text += $"Client Connected @ {ep.Address} : {ep.Port}\n";
            }
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