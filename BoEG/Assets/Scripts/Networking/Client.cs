using System.Diagnostics;
using System.Net;
using Networking.Messages;

namespace Networking
{
    public class Client : Base
    {
        public Client(int port = 8888) : base(port)
        {
        }

        private IPEndPoint _connectedTo;
        private ConnectionState _connectionState;

        private void Connect(IPEndPoint endpoint)
        {
            Debug.Assert(_connectionState == ConnectionState.Disconnected);
            GenerateSalt();
            _connectedTo = endpoint;
            _connectionState = ConnectionState.ConnectingRequest;

            SendMessage(new ConnectMessage(Salt), _connectedTo);
        }

        public void Connect(IPAddress address)
        {
            Connect(CreateEndpoint(address));
        }

        public void Connect(string address)
        {
            Connect(CreateEndpoint(address));
        }

        public void Disconnect()
        {
            //TODO
            //Fire off courtesy disconnect packets
            _connectedTo = null;
            _connectionState = ConnectionState.Disconnected;
        }
    }
}