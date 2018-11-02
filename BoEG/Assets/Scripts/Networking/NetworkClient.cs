//using System.Net;
//
//namespace Networking
//{
//    public class NetworkClient
//    {
//        public NetworkClient(IPEndPoint local)
//        {
//            _transport = new Transport(local);
//            _host = null;
//        }
//
//        public void Connect(IPEndPoint host)
//        {
//            _host = host;
//        }
//
//        private readonly Transport _transport;
//        private IPEndPoint _host;
//
//        public void Send(byte[] buffer)
//        {
//            _transport.Send(buffer, _host);
//        }
//
//        //TODO return error code
//        public void Recieve(byte[] buffer, out IPEndPoint endPoint)
//        {
//            _transport.Recieve(buffer, out endPoint);
//            if (endPoint.Equals(_host)) return;
//            for (var i = 0; i < buffer.Length; i++) //Dump payload
//                buffer[i] = 0;
//        }
//    }
//}