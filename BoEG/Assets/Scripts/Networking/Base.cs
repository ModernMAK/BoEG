using System;
using System.Net;
using System.Net.Sockets;
using Networking.Messages;
using UnityEngine.Assertions;

namespace Networking
{
    public class Base
    {
        protected Base(int port = 8888)
        {
            _saltGenerator = new Random();
            _salt = 0;
            _port = port;
            _udpClient = new UdpClient(port);
        }

        private readonly Random _saltGenerator;
        private ulong _salt;
        private readonly int _port;
        private readonly UdpClient _udpClient;


        protected void GenerateSalt()
        {
            _salt = _saltGenerator.NextULong();
        }

        protected IPEndPoint CreateEndpoint(IPAddress address)
        {
            return new IPEndPoint(address, _port);
        }

        protected IPEndPoint CreateEndpoint(string address)
        {
            var addressParsed = Dns.GetHostEntry(address).AddressList[0];
            return CreateEndpoint(addressParsed);
        }


        protected UdpClient UdpClient
        {
            get { return _udpClient; }
        }

        protected Socket Socket
        {
            get { return _udpClient.Client; }
        }

        protected int Port
        {
            get { return _port; }
        }

        protected ulong Salt
        {
            get { return _salt; }
        }

        //Sends the message
        protected void SendMessage(Message msg, IPEndPoint address)
        {
            //
            var bytes = msg.Write();
            _udpClient.Send(bytes, bytes.Length, address);
        }


        protected byte[] FetchMessage(out IPEndPoint endPoint)
        {
            endPoint = new IPEndPoint(IPAddress.Any, 0);
            return _udpClient.Receive(ref endPoint);
        }

        //Recieves the next available Message
        protected Message RecieveMessage(out IPEndPoint sender)
        {
            var bytes = FetchMessage(out sender);
            if (bytes.Length == 0)
                return new Message(); //Error message
            var msgType = (MessageType) bytes[0];
            Message msg;
            switch (msgType)
            {
                case MessageType.Connect:
                    msg = new ConnectMessage();
                    break;
                case MessageType.Disconnect:
                    msg = new Message();
                    break;
                case MessageType.Data:
                    msg = new Message();
                    break;
                case MessageType.Error:
                    msg = new Message();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            msg.Read(bytes);
            return msg;
        }

        public struct Connection
        {
            public Connection(IPEndPoint address, ulong salt) : this()
            {
                Address = address;
                Salt = salt;
            }

            public IPEndPoint Address { get; private set; }
            public ulong Salt { get; private set; }

            public bool Connected
            {
                get { return Address != null; }
            }

            public Connection SetSalt(ulong salt)
            {
                return new Connection(Address, salt);
            }
        }
    }

    //
//    public struct PacketHeader
//    {
//        private PacketHeader(
//            uint crc, ushort sequence, ushort ack, byte ackBits,
//            byte packetType,
//            byte fragmentId, byte numFragments) : this()
//        {
//            Crc = crc;
//            Sequence = sequence;
//            PacketType = packetType;
//            FragmentId = 0;
//            NumFragments = 0;
//            Ack = ack;
//            AckBits = ackBits;
//            FragmentId = fragmentId;
//            NumFragments = numFragments;
//        }
//
//        public PacketHeader(uint crc, ushort sequence, ushort ack, byte ack_bits, byte packetType) :
//            this(crc, sequence, ack, ack_bits, packetType, 0, 0)
//        {
//        }
//
//        public PacketHeader(uint crc, ushort sequence, ushort ack, byte ack_bits, byte fragmentId, byte numFragments) :
//            this(crc, sequence, ack, ack_bits, 0, fragmentId, numFragments)
//        {
//        }
//
//
//        public uint Crc { get; private set; }
//        public ushort Sequence { get; private set; }
//        public ushort Ack { get; private set; }
//        public byte AckBits { get; private set; }
//
//        public byte PacketType { get; private set; }
//        public byte FragmentId { get; private set; }
//        public byte NumFragments { get; private set; }
//    }
//
//    public struct PacketData
//    {
//        public PacketData(bool acked) : this()
//        {
//            Acked = acked;
//        }
//
//        public bool Acked { get; private set; }
//    }
//
//
//    public class SequenceBuffer
//    {
//        public bool GetPacket(uint sequence, out PacketData data)
//        {
//            var index = sequence % packetBuffer.Length;
//            if (sequenceBuffer[index] == sequence)
//            {
//                data = packetBuffer[index];
//                return true;
//            }
//            else
//            {
//                data = default(PacketData);
//                return false;
//            }
//        }
//
//        public ushort GetAck()
//        {
//            return lastAck;
//        }
//
//        public byte GetAckBits(ushort sequence)
//        {
//            byte ackBits = 0;
//            for (var i = 0; i < 32; i++)
//            {
//                var index = (sequence - i + sequenceBuffer.Length) % sequenceBuffer.Length;
//                if (packetBuffer[index].Acked)
//                    ackBits |= (byte) (1 << i);
//            }
//
//            return ackBits;
//        }
//
//        public void InsertPacket(ushort sequence, PacketData data)
//        {
//            var index = sequence % packetBuffer.Length;
//            lastAck = sequence;
//            sequenceBuffer[index] = sequence;
//            packetBuffer[index] = data;
//        }
//
//        private readonly ushort[] sequenceBuffer;
//        private readonly PacketData[] packetBuffer;
//        private ushort lastAck;
//    }
//
//
//    public class NetworkAgent
//    {
//        private SequenceBuffer buffer;
//
//        private ushort sequence;
//
//        private void Send()
//        {
//            var packetData = new PacketData(false);
//            buffer.InsertPacket(sequence, packetData);
//            var packet = new PacketHeader(0, sequence, buffer.GetAck(), buffer.GetAckBits(), 0);
//            if (sequence == ushort.MaxValue)
//                sequence = 0;
//            else sequence++;
//            //Send packet
//            
//            
//        }
//
//        private void Recieve()
//        {
//            var header = ReadPacketHeader();
//            var packetData = new PacketData(true);
//            buffer.InsertPacket(header.Sequence,packetData);
//            header
//        }
//
//        private PacketHeader ReadPacketHeader()
//        {
//            return new PacketHeader();
//        }
//    }
//
//    public class NetworkServer
//    {
//    }
//
//    public class NetworkClient
//    {
//    }

//    public static class NetworkTester
//    {
//        public static void Test()
//        {
//            NetworkBase @base = new NetworkBase();
//            NetworkClient client = new NetworkClient();
//
//            UdpClient
//
//            @base.Start();
//            client.Connect();
//            var serverClient = @base._listener.AcceptTcpClient();
//
//            var serverMsg = "Hi Client, I'm Server";
//            var clientMsg = "Hi Server, I'm Client";
//            var buffer = new byte[100000];
//            using (var serializer = new Serializer())
//            {
//                serializer.Write(serverMsg);
//                @base.Send(serializer.Buffer(), serverClient);
//                client.Recieve(buffer);
//                using (var deserializer = new Deserializer(buffer))
//                {
//                    var msg = deserializer.ReadString();
//                    Debug.Log(msg);
//                }
//            }
//
//            using (var serializer = new Serializer())
//            {
//                serializer.Write(clientMsg);
//                client.Send(serializer.Buffer());
//                @base.Recieve(buffer, serverClient);
//                using (var deserializer = new Deserializer(buffer))
//                {
//                    var msg = deserializer.ReadString();
//                    Debug.Log(msg);
//                }
//            }
//
//            serverClient.Close();
//            @base.Close();
//            client.Disconnect();
//        }
//    }
//
//    public class NetworkBase : IDisposable
//    {
//        public NetworkBase(int port) : this(IPAddress.Any, port)
//        {
//        }
//
//        public NetworkBase(string address, int port) : this(IPAddress.Parse(address), port)
//        {
//        }
//
//        public NetworkBase(IPAddress address, int port)
//        {
//            _port = port;
//            _self = new UdpClient(port);
//            _endPoint = new IPEndPoint(address, port);
//        }
//
//        private readonly UdpClient _self;
//        private IPEndPoint _endPoint;
//        private readonly int _port;
//
//
//        protected byte[] Recieve(out NetworkMessage msgType)
//        {
//            var data = _self.Receive(ref _endPoint);
//            using (var stream = new MemoryStream(data))
//            {
//                using (var reader = new BinaryReader(stream))
//                {
//                    msgType = (NetworkMessage) reader.ReadByte();
//                    data = reader.ReadBytes(data.Length - 1);
//                    return data;
//                }
//            }
//        }
//
//        protected int Send(NetworkMessage msgType, byte[] data, string address)
//        {
//            var ep = new IPEndPoint(IPAddress.Parse(address), _port);
//            return Send(msgType, data, ep);
//        }
//
//        protected int Send(NetworkMessage msgType, byte[] data, IPEndPoint address)
//        {
//            using (var stream = new MemoryStream())
//            {
//                using (var writer = new BinaryWriter(stream))
//                {
//                    writer.Write((byte)msgType);
//                    writer.Write(data);
//                    data = stream.ToArray();
//                    return _self.Send(data, data.Length, address);
//                }
//            }
//        }
//
//
//        protected void Close()
//        {
//            _self.Close();
//        }
//
//        public virtual void Dispose()
//        {
//            if (_self != null) ((IDisposable) _self).Dispose();
//        }
//    }
//
//    public enum NetworkMessage
//    {
//        Connection = 0,
//    }
//    
//    public class NetworkServer : NetworkBase
//    {
//        public NetworkServer(int port) : base(port)
//        {
//        }
//
//        public NetworkServer(string address, int port) : base(address, port)
//        {
//        }
//
//        public NetworkServer(IPAddress address, int port) : base(address, port)
//        {
//        }
//        
//        
//        
//        
//    }
//
//    public class NetworkClient : NetworkBase
//    {
//        public NetworkClient(int port) : base(port)
//        {
//        }
//
//        public NetworkClient(string address, int port) : base(address, port)
//        {
//        }
//
//        public NetworkClient(IPAddress address, int port) : base(address, port)
//        {
//        }
//
//
//        public void Connect(string ipAddress = "127.0.0.1", int port = 8888)
//        {
//            var parsedIpAddress = IPAddress.Parse(ipAddress);
//            _client.Connect(parsedIpAddress, port);
//        }
//
//        public void Send(byte[] buffer)
//        {
//            var stream = _client.GetStream();
//            Write(stream, buffer);
//        }
//
//        public void Recieve(byte[] buffer)
//        {
//            var stream = _client.GetStream();
//            Read(stream, buffer, _client.ReceiveBufferSize);
//        }
//
//        public byte[] Recieve()
//        {
//            var stream = _client.GetStream();
//            return Read(stream, _client.ReceiveBufferSize);
//        }
//
//        public void Disconnect()
//        {
//            _client.Close();
//        }
//    }
}