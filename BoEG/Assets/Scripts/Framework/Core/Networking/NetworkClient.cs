using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace Framework.Core.Networking
{
    public class NetworkClient
    {
        public static int MessageBufferSize => NetworkServer.MessageBufferSize;
        private static readonly byte[] MessageBuffer = new byte[MessageBufferSize];
        
        public NetworkClient() : this(new TcpClient())
        {
        }

        public NetworkClient(TcpClient client)
        {
            _client = client;
        }

        private readonly TcpClient _client;
        public TcpClient Client => _client;


        //This 
        public bool TryConnect(IPEndPoint endPoint)
        {
            try
            {
                Connect(endPoint);
                return true;
            }
            catch (SocketException ex)
            {
                Debug.Log(ex.Message);
                return false;
            }
        }
        public void Connect(IPEndPoint endPoint)
        {
            _client.Connect(endPoint);
            OnClientConnected(new Tuple<NetworkClient>(this));
        }
        public bool Connected => _client.Connected;
        public void Close() => _client.Close();
    
        
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

        public void ReadMessages()
        {
            using (var memory = new MemoryStream(MessageBufferSize))
            {
                ReadMessage(memory);
            }
            
        }


        private bool ReadMessage(MemoryStream stream)
        {
            if (!Client.Connected)
                return false;
            using (var net = Client.GetStream())
            {
                if (!TryReadMessage(net, stream))
                    return false;
                OnMessageReceived(new Tuple<NetworkClient, MemoryStream>(this, stream));
                return true;
            }
        }

        public void WriteMessage(MemoryStream stream)
        {
            using (var netStream = Client.GetStream())
            {
                WriteMessage(netStream, stream);
                OnMessageSent(new Tuple<NetworkClient, MemoryStream>(this, stream));
            }
        }
        
        public event EventHandler<Tuple<NetworkClient>> ClientConnected;
        public event EventHandler<Tuple<NetworkClient>> ClientDisconnected;

        public event EventHandler<Tuple<NetworkClient, MemoryStream>> MessageReceived;
        public event EventHandler<Tuple<NetworkClient, MemoryStream>> MessageSent;

        
        
        protected virtual void OnMessageReceived(Tuple<NetworkClient, MemoryStream> e)
        {
            MessageReceived?.Invoke(this, e);
        }

        protected virtual void OnMessageSent(Tuple<NetworkClient, MemoryStream> e)
        {
            MessageSent?.Invoke(this, e);
        }
        
        protected virtual void OnClientConnected(Tuple<NetworkClient> e)
        {
            ClientConnected?.Invoke(this, e);
        }

        protected virtual void OnClientDisconnected(Tuple<NetworkClient> e)
        {
            ClientDisconnected?.Invoke(this, e);
        }
    }
}