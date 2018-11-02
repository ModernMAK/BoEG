using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using Networking;
using UnityEngine;

namespace Networking
{
    public class Transport : IDisposable
    {
        public Transport(IPEndPoint localPoint)
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _socket.Bind(localPoint);
        }

        private readonly Socket _socket;

        public int Port
        {
            get { return ((IPEndPoint) _socket.LocalEndPoint).Port; }
        }

        public IPAddress Address
        {
            get { return ((IPEndPoint) _socket.LocalEndPoint).Address; }
        }


        public void Send(byte[] buffer, IPEndPoint endPoint)
        {
            var read = _socket.SendTo(buffer, endPoint);
        }

        public void Recieve(byte[] buffer, out IPEndPoint endPoint)
        {
            endPoint = new IPEndPoint(IPAddress.Any, 0);
            var temp = (EndPoint) endPoint;
            var read = _socket.ReceiveFrom(buffer, SocketFlags.None, ref temp);
        }

        public void Dispose()
        {
            _socket.Dispose();
        }
    }

}

//
//    private class Channel
//    {
//        //Channel houses 4 subchannels, each subchannel is either Fragmented or Unfragmented
//        //Reliable - Sent until Acknowledged
//        //Unreliable - Sent once
//
//        //Ordered (Reliable - Stalls until Acknowledged) (Unreliable - Drops Older Messages Recieved)
//        //Unordered (Reliable - Resends all until Acknowledged) (Unreliable - Follow Other Rules)
//
//        //Fragmented (Sends all fragments until acknowledge, follows Ordered / Unordered rules)
//        //Unfragmented - Follow Other Rules
//
//
//        private const ushort MAX_UDP_BUFFER_SIZE = 65507; //
//        private const ushort CHANNEL_HEADER = 0;
//        private const ushort MAX_BUFFER_SIZE = MAX_UDP_BUFFER_SIZE - CHANNEL_HEADER;
//
//        public Channel(ushort bufferSize, Transport transport)
//        {
//            if (bufferSize > MAX_BUFFER_SIZE)
//                throw new ArgumentException(
//                    "The maximum size of a udp message is " + MAX_BUFFER_SIZE +
//                    ", messages requiring a larger buffer will be fragmented. (UDP only allows " + MAX_UDP_BUFFER_SIZE +
//                    ", and Channels require " + CHANNEL_HEADER + ")",
//                    "bufferSize");
//            _buffer = new byte[bufferSize];
//            _transport = transport;
//        }
//
//        private Transport _transport;
//        private readonly byte[] _buffer;
//        private ushort _sequence;
//
//
//        [Flags]
//        private enum PacketType : byte
//        {
//            Ordered = (1 << 0),
//            Reliable = (1 << 1),
//            Fragmented = (1 << 2)
//        }
//
//        private class Packet
//        {
//            public Packet()
//            {
//                Crc = Sequence = FragmentId = NumFragment = 0;
//
//                PacketType = 0;
//                Payload = new byte[0];
//            }
//
//            public Packet(ushort sequence, ushort ack, uint ackFlags, PacketType type, byte fragmentId,
//                byte numFragments, byte[] payload)
//            {
//                Sequence = sequence;
//                Acknowledgement = ack;
//                AcknowledgementFlags = ackFlags;
//                PacketType = type;
//                FragmentId = fragmentId;
//                NumFragment = numFragments;
//                Payload = payload;
//                GenerateCrc();
//            }
//
//            public int Crc { get; private set; }
//
//            //Required for all Packets (Ordered / Reliable)
//            public ushort Sequence { get; private set; }
//
//            public ushort Acknowledgement { get; private set; }
//
//            public uint AcknowledgementFlags { get; private set; }
//
//            //Required
//            public PacketType PacketType { get; private set; }
//
//            //Required For Fragmented
//            public byte FragmentId { get; private set; }
//            public byte NumFragment { get; private set; }
//            public byte[] Payload { get; private set; }
//
//
//            //Crc (int32) + PayloadSize & Sequence (int16 x2) + PacketType (byte)
//            public const int SHARED_HEADER_SIZE = sizeof(int) + sizeof(ushort) * 2 + sizeof(byte);
//
//            //Shared + FragmentId & NumFragments (byte x2)
//            public const int FRAGMENTED_HEADER_SIZE = SHARED_HEADER_SIZE + sizeof(byte) * 2;
//
//            //Shared + FragmentId & NumFragments (byte x2)
//            public const int RELIABLE_OR_ORDERED_HEADER_SIZE = SHARED_HEADER_SIZE + sizeof(ushort) + sizeof(uint);
//
//
//            //Shared + FragmentId & NumFragments (byte x2)
//            public const int FRAGMENTED_RELIABLE_OR_ORDERED_HEADER_SIZE =
//                FRAGMENTED_HEADER_SIZE + RELIABLE_OR_ORDERED_HEADER_SIZE - SHARED_HEADER_SIZE;
//
//            public static Packet ReadPacket(byte[] buffer, out byte errorCode)
//            {
//                var packet = new Packet();
//                using (var stream = new MemoryStream(buffer))
//                {
//                    using (var reader = new BinaryReader(stream))
//                    {
//                        packet.Crc = reader.ReadInt32();
//                        packet.Sequence = reader.ReadUInt16();
//                        packet.PacketType = (PacketType) reader.ReadByte();
//                        if ((packet.PacketType & PacketType.Ordered) == PacketType.Ordered ||
//                            (packet.PacketType & PacketType.Reliable) == PacketType.Reliable)
//                        {
//                            packet.Acknowledgement = reader.ReadUInt16();
//                            packet.AcknowledgementFlags = reader.ReadUInt32();
//                        }
//
//                        if ((packet.PacketType & PacketType.Fragmented) == PacketType.Fragmented)
//                        {
//                            packet.FragmentId = reader.ReadByte();
//                            packet.NumFragment = reader.ReadByte();
//                        }
//
//                        var payloadSize = reader.ReadUInt16();
//                        packet.Payload = reader.ReadBytes(payloadSize);
//                    }
//                }
//
//                errorCode = 0;
//                if (packet.CalculateCrc() != packet.Crc)
//                {
//                    errorCode = 1; //CRC Mismatch
//                    return new Packet();
//                }
//                else
//                {
//                    return packet;
//                }
//            }
//
//            public void WritePacket(byte[] buffer)
//            {
//                byte[] temp = Output(true);
//                temp.CopyTo(buffer, 0);
//            }
//
//            private byte[] Output(bool includeCrc = true)
//            {
//                var size = Payload.Length;
//
//                var fragmented = ((PacketType & PacketType.Fragmented) == PacketType.Fragmented);
//                var reliableOrOrdered = ((PacketType & PacketType.Ordered) == PacketType.Ordered) ||
//                                        ((PacketType & PacketType.Reliable) == PacketType.Reliable);
//
//                if (reliableOrOrdered && fragmented)
//                    size += FRAGMENTED_RELIABLE_OR_ORDERED_HEADER_SIZE;
//                else if (reliableOrOrdered)
//                    size += RELIABLE_OR_ORDERED_HEADER_SIZE;
//                else if (fragmented)
//                    size += FRAGMENTED_HEADER_SIZE;
//
//
//                if (!includeCrc)
//                    size--; //Remove CRC
//
//
//                var buffer = new byte[size];
//                using (var stream = new MemoryStream(buffer))
//                {
//                    using (var writer = new BinaryWriter(stream))
//                    {
//                        if (includeCrc)
//                            writer.Write(Crc);
//                        writer.Write(Sequence);
//                        writer.Write((byte) PacketType);
//                        if (reliableOrOrdered)
//                        {
//                            writer.Write(Acknowledgement);
//                            writer.Write(AcknowledgementFlags);
//                        }
//
//                        if (fragmented)
//                        {
//                            writer.Write(FragmentId);
//                            writer.Write(NumFragment);
//                        }
//
//                        writer.Write((short) Payload.Length);
//                        writer.Write(Payload);
//                    }
//
//                    return stream.ToArray();
//                }
//            }
//
//
//            //Applies the Calculated CRC
//            public void GenerateCrc()
//            {
//                Crc = CalculateCrc();
//            }
//
//            //Does not apply the CRC only calculates
//            public int CalculateCrc()
//            {
//                //Exclude Crc when calculating CRC
//                return CalculateCrc(Output(false));
//            }
//
//            public static int CalculateCrc(byte[] buffer)
//            {
//                int crc = 0;
//                for (var i = 0; i < Mathf.CeilToInt(buffer.Length / 4f); i++)
//                {
//                    int chunk = 0;
//                    for (var j = 0; j < 4; j++)
//                        chunk |= (buffer.Length <= 4 * i + j) ? 0 : buffer[4 * i + 1] << (32 - (j + 1) * 8);
//                    crc ^= chunk;
//                }
//
//                return crc;
//            }
//        }
//
//
//        public void SendFragmented(byte[] payload)
//        {
//            //Header 
//            var maxPayloadSize = _buffer.Length - Packet.FRAGMENTED_HEADER_SIZE;
//            var fragments = payload.Length / maxPayloadSize;
//            if (fragments > byte.MaxValue)
//                throw new ArgumentException("Payload cannot be made into fragments of size.");
//            for (var i = 0; i < fragments; i++)
//            {
//                var payloadSize = Mathf.Min(maxPayloadSize, payload.Length - (i * maxPayloadSize));
//                var packetPayload = new byte[payloadSize];
//                for (var j = 0; j < payloadSize; j++)
//                    packetPayload[j] = payload[i * maxPayloadSize + j];
//
//                var packet = new Packet(_sequence, 0, 0, PacketType.Fragmented, (byte) i, (byte) fragments,
//                    packetPayload);
//                AdvanceSequence();
//            }
//        }
//
//        private void Send(Packet packet, IPEndPoint reciever)
//        {
//            packet.WritePacket(_buffer);
//            _transport.Send(_buffer, reciever);
//        }
//
//        private Packet Recieve(out IPEndPoint sender, out byte packetError)
//        {
//            _transport.Recieve(_buffer, out sender);
//            return Packet.ReadPacket(_buffer, out packetError);
//        }
//
//        private void AdvanceSequence()
//        {
//            if (_sequence == ushort.MaxValue)
//                _sequence = 0;
//            else
//            {
//                _sequence++;
//            }
//        }
//    }