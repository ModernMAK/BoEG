using System.Net.Sockets;

namespace MobaGame.Framework.Core.Networking.LLAPI
{
    public abstract class NetworkStreamTransport
    {
        public const int MessageBufferSize = 1024;
        private static readonly byte[] MessageBuffer = new byte[MessageBufferSize];

        public static void ReadMessage(NetworkStream netStream, System.IO.Stream memStream, out bool read)
        {
            read = netStream.DataAvailable;
            while (netStream.DataAvailable)
            {
                var bytesRead = netStream.Read(MessageBuffer, 0, MessageBufferSize);
                memStream.Write(MessageBuffer, 0, bytesRead);
            }
        }

        public static bool TryReadMessage(TcpClient client, System.IO.Stream memoryStream, out bool read)
        {
            if (client.Connected)
            {
                TryReadMessage(client.GetStream(), memoryStream, out read);
                return true;
            }

            read = false;
            return false;
        }

        public static void TryReadMessage(NetworkStream netStream, System.IO.Stream memoryStream, out bool read)
        {
            try
            {
                ReadMessage(netStream, memoryStream, out read);
            }
            catch (SocketException)
            {
                //tODO log    
                read = false;
            }
        }

        public static bool TryWriteMessage(TcpClient client, System.IO.Stream memoryStream)
        {
            if (client.Connected)
            {
                return TryWriteMessage(client.GetStream(), memoryStream);
            }

            return false;
        }

        public static bool TryWriteMessage(NetworkStream netStream, System.IO.Stream memStream)
        {
            try
            {
                return WriteMessage(netStream, memStream);
            }
            catch (SocketException)
            {
                //TODO log exception
                return false;
            }
        }

        //Returns false if nothing was written
        public static bool WriteMessage(NetworkStream netStream, System.IO.Stream memStream)
        {
            var hasRun = false;
            while (true)
            {
                var bytesRead = memStream.Read(MessageBuffer, 0, MessageBufferSize);
                if (bytesRead > 0)
                {
                    netStream.Write(MessageBuffer, 0, bytesRead);
                    hasRun = true;
                }
                else
                    return hasRun;
            }
        }

        protected NetworkStreamTransport()
        {
        }
    }
}