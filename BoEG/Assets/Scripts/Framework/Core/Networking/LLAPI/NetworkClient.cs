using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using MobaGame.Framework.Core.Networking.Stream;

namespace MobaGame.Framework.Core.Networking.LLAPI
{
    public class NetworkClient : NetworkStreamTransport
    {
        public NetworkClient() : this(new TcpClient())
        {
        }

        public NetworkClient(TcpClient client)
        {
            _client = client;
        }

        private readonly TcpClient _client;
        
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
                UnityEngine.Debug.Log(ex.Message);
                return false;
            }
        }

        public EndPoint RemoteEndPoint => _client.Connected ? _client.Client.RemoteEndPoint : null;
        public EndPoint LocalEndPoint => _client.Connected ? _client.Client.LocalEndPoint : null;

        public void Connect(IPEndPoint endPoint)
        {
            _client.Connect(endPoint);
            OnClientConnected();
        }

        public bool Connected => _client.Connected;

        public void Close()
        {
            _client.Close();
            OnClientDisconnected();
        }


        public void ReadAllMessages()
        {
            using (var memory = new MemoryStream(MessageBufferSize))
            {
                while (ReadMessage(memory))
                {
                    memory.SetLength(0); //Clears memory stream
                    memory.Seek(0, SeekOrigin.Begin); //Points stream to beginning
                }
            }
        }


        public bool ReadMessage(System.IO.Stream stream)
        {
            if (TryReadMessage(_client, stream, out var read))
            {
                if (read)
                    OnMessageReceived(new ReadOnlyStream(stream));
            }

            return read;
        }

        public bool WriteMessage(System.IO.Stream stream)
        {
            if (TryWriteMessage(_client, stream))
            {
                OnMessageSent(new ReadOnlyStream(stream));
                return true;
            }

            return false;
        }

        public event EventHandler ClientConnected;
        public event EventHandler ClientDisconnected;

        public event EventHandler<System.IO.Stream> MessageReceived;
        public event EventHandler<System.IO.Stream> MessageSent;


        protected virtual void OnMessageReceived(System.IO.Stream e)
        {
            MessageReceived?.Invoke(this, e);
        }

        protected virtual void OnMessageSent(System.IO.Stream e)
        {
            MessageSent?.Invoke(this, e);
        }

        protected virtual void OnClientConnected()
        {
            ClientConnected?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnClientDisconnected()
        {
            ClientDisconnected?.Invoke(this, EventArgs.Empty);
        }

        public bool CheckServer() => _client.Connected;
    }
}