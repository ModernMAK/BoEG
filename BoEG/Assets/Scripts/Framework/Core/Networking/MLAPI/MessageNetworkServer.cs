using System.Net;
using System.Net.Sockets;

namespace Framework.Core.Networking.MLAPI
{
    public class MessageNetworkServer : NetworkServer
    {
        public MessageNetworkServer(IPEndPoint endPoint) : base(endPoint)
        {
        }

        public MessageNetworkServer(TcpListener listener) : base(listener)
        {
        }
    }
}