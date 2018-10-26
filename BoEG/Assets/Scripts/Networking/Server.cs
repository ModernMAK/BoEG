using System.Net;
using Networking.Messages;

namespace Networking
{
    public class Server : Base
    {
        public Server(int maxClients, int port = 8888) : base(port)
        {
            _clients = new Connection[maxClients];
        }

        private readonly Connection[] _clients;

        private int GetNextClient()
        {
            for (var i = 0; i < _clients.Length; i++)
                if (!_clients[i].Connected)
                    return i;
            return -1;
        }


        private bool IsConnected(int clientIndex)
        {
            return _clients[clientIndex].Connected;
        }

        private IPEndPoint GetAddress(int clientIndex)
        {
            return _clients[clientIndex].Address;
        }

        private void AcceptConnection(IPEndPoint client, ConnectMessage msg)
        {
            var nextClientSlot = GetNextClient();
            if (nextClientSlot == -1)
                SendMessage(new DisconnectMessage(), client);
            else
            {
                _clients[nextClientSlot] = new Connection(client, msg.Salt);
                SendMessage(new ConnectMessage(Salt), client);
            }
        }

        public void Send(Message msg)
        {
            foreach (var conn in _clients)
            {
                if (conn.Connected)
                    SendMessage(msg, conn.Address);
            }
        }

        //Closes all open connections
        public void CloseConnections()
        {
            for (var i = 0; i < _clients.Length; i++)
            {
                var conn = _clients[i];
                if (conn.Connected)
                    SendMessage(new DisconnectMessage(), conn.Address);
                _clients[i] = new Connection();
            }
        }
    }
}