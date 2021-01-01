using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace Framework.Core.Networking
{
    public abstract class NetworkTransport
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
                ReadMessage(client.GetStream(), memoryStream, out read);
                return true;
            }

            read = false;
            return false;
        }

        public static bool TryWriteMessage(TcpClient client, Stream memoryStream)
        {
            if (client.Connected)
            {
                return WriteMessage(client.GetStream(), memoryStream);
            }

            return false;
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

        protected NetworkTransport()
        {
        }
    }
}