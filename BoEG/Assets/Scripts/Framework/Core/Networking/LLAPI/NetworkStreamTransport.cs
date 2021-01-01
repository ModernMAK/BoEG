using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace Framework.Core.Networking
{
    public abstract class NetworkStreamTransport
    {
        public const int MessageBufferSize = 1024;
        private static readonly byte[] MessageBuffer = new byte[MessageBufferSize];

        public static void ReadMessage(NetworkStream netStream, Stream memStream, out bool read)
        {
            read = netStream.DataAvailable;
            while (netStream.DataAvailable)
            {
                var bytesRead = netStream.Read(MessageBuffer, 0, MessageBufferSize);
                memStream.Write(MessageBuffer, 0, bytesRead);
            }
        }

        public static bool TryReadMessage(TcpClient client, Stream memoryStream, out bool read)
        {
            if (client.Connected)
            {
                TryReadMessage(client.GetStream(), memoryStream, out read);
                return true;
            }

            read = false;
            return false;
        }

        public static void TryReadMessage(NetworkStream netStream, Stream memoryStream, out bool read)
        {
            try
            {
                ReadMessage(netStream, memoryStream, out read);
            }
            catch (SocketException exception)
            {
                //tODO log    
                read = false;
            }
        }

        public static bool TryWriteMessage(TcpClient client, Stream memoryStream)
        {
            if (client.Connected)
            {
                return TryWriteMessage(client.GetStream(), memoryStream);
            }

            return false;
        }

        public static bool TryWriteMessage(NetworkStream netStream, Stream memStream)
        {
            try
            {
                return WriteMessage(netStream, memStream);
            }
            catch (SocketException e)
            {
                //TODO log exception
                return false;
            }
        }

        //Returns false if nothing was written
        public static bool WriteMessage(NetworkStream netStream, Stream memStream)
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