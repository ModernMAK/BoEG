using System;
using System.IO;
using UnityEngine;

namespace Networking
{
    public class Packet
    {
        public Packet()
        {
            Crc = Sequence = FragmentId = NumFragment = 0;

            PacketType = 0;
            Payload = new byte[0];
        }

        public Packet(ushort sequence, ushort ack, uint ackFlags, PacketType type, byte[] payload)
        {
            Sequence = sequence;
            Acknowledgement = ack;
            AcknowledgementFlags = ackFlags;
            PacketType = type;
//            FragmentId = fragmentId;
//            NumFragment = numFragments;
            Payload = payload;
            GenerateCrc();
        }

        public Packet SetCrc(int crc)
        {
            return new Packet()
            {
                Crc = crc,
                Sequence = Sequence,
                Acknowledgement = Acknowledgement,
                AcknowledgementFlags = AcknowledgementFlags,
                FragmentId = FragmentId,
                NumFragment = NumFragment,
                PacketType = PacketType,
                Payload = Payload
            };
        }
        public int Crc { get; private set; }

        //Required for all Packets (Ordered / Reliable)
        public ushort Sequence { get; private set; }
        public Packet SetSequence(ushort sequence)
        {
            return new Packet()
            {
                Crc = Crc,
                Sequence = sequence,
                Acknowledgement = Acknowledgement,
                AcknowledgementFlags = AcknowledgementFlags,
                FragmentId = FragmentId,
                NumFragment = NumFragment,
                PacketType = PacketType,
                Payload = Payload
            };
        }

        public ushort Acknowledgement { get; private set; }
        public Packet SetAcknowledgement(ushort acknowledgement)
        {
            return new Packet()
            {
                Crc = Crc,
                Sequence = Sequence,
                Acknowledgement = acknowledgement,
                AcknowledgementFlags = AcknowledgementFlags,
                FragmentId = FragmentId,
                NumFragment = NumFragment,
                PacketType = PacketType,
                Payload = Payload
            };
        }

        public uint AcknowledgementFlags { get; private set; }
        public Packet SetAcknowledgementFlags(uint acknowledgementFlags)
        {
            return new Packet()
            {
                Crc = Crc,
                Sequence = Sequence,
                Acknowledgement = Acknowledgement,
                AcknowledgementFlags = acknowledgementFlags,
                FragmentId = FragmentId,
                NumFragment = NumFragment,
                PacketType = PacketType,
                Payload = Payload
            };
        }

        //Required
        public PacketType PacketType { get; private set; }
        public Packet SetPacketType(PacketType packetType)
        {
            return new Packet()
            {
                Crc = Crc,
                Sequence = Sequence,
                Acknowledgement = Acknowledgement,
                AcknowledgementFlags = AcknowledgementFlags,
                FragmentId = FragmentId,
                NumFragment = NumFragment,
                PacketType = packetType,
                Payload = Payload
            };
        }

        //Required For Fragmented
        public byte FragmentId { get; private set; }
        public Packet SetFragmentId(byte fragmentId)
        {
            return new Packet()
            {
                Crc = Crc,
                Sequence = Sequence,
                Acknowledgement = Acknowledgement,
                AcknowledgementFlags = AcknowledgementFlags,
                FragmentId = fragmentId,
                NumFragment = NumFragment,
                PacketType = PacketType,
                Payload = Payload
            };
        }
        public byte NumFragment { get; private set; }
        public Packet SetNumFragment(byte numFragment)
        {
            return new Packet()
            {
                Crc = Crc,
                Sequence = Sequence,
                Acknowledgement = Acknowledgement,
                AcknowledgementFlags = AcknowledgementFlags,
                FragmentId = FragmentId,
                NumFragment = numFragment,
                PacketType = PacketType,
                Payload = Payload
            };
        }
        public byte[] Payload { get; private set; }
        public Packet SetPayload(byte[] payload)
        {
            return new Packet()
            {
                Crc = Crc,
                Sequence = Sequence,
                Acknowledgement = Acknowledgement,
                AcknowledgementFlags = AcknowledgementFlags,
                FragmentId = FragmentId,
                NumFragment = NumFragment,
                PacketType = PacketType,
                Payload = payload
            };
        }


        //Crc (int32) + PayloadSize & Sequence (int16 x2) + PacketType (byte)
        public const int SHARED_HEADER_SIZE = sizeof(int) + sizeof(ushort) * 2 + sizeof(byte);

        //Shared + FragmentId & NumFragments (byte x2)
        public const int FRAGMENTED_HEADER_SIZE = SHARED_HEADER_SIZE + sizeof(byte) * 2;

        //Shared + FragmentId & NumFragments (byte x2)
        public const int RELIABLE_OR_ORDERED_HEADER_SIZE = SHARED_HEADER_SIZE + sizeof(ushort) + sizeof(uint);


        //Shared + FragmentId & NumFragments (byte x2)
        public const int FRAGMENTED_RELIABLE_OR_ORDERED_HEADER_SIZE =
            FRAGMENTED_HEADER_SIZE + RELIABLE_OR_ORDERED_HEADER_SIZE - SHARED_HEADER_SIZE;

        public static Packet ReadPacket(byte[] buffer, out byte errorCode)
        {
            var packet = new Packet();
            using (var stream = new MemoryStream(buffer))
            {
                using (var reader = new BinaryReader(stream))
                {
                    packet.Crc = reader.ReadInt32();
                    packet.Sequence = reader.ReadUInt16();
                    packet.PacketType = (PacketType) reader.ReadByte();
                    if ((packet.PacketType & PacketType.Ordered) == PacketType.Ordered ||
                        (packet.PacketType & PacketType.Reliable) == PacketType.Reliable)
                    {
                        packet.Acknowledgement = reader.ReadUInt16();
                        packet.AcknowledgementFlags = reader.ReadUInt32();
                    }

                    if ((packet.PacketType & PacketType.Fragmented) == PacketType.Fragmented)
                    {
                        packet.FragmentId = reader.ReadByte();
                        packet.NumFragment = reader.ReadByte();
                    }

                    var payloadSize = reader.ReadUInt16();
                    packet.Payload = reader.ReadBytes(payloadSize);
                }
            }

            errorCode = 0;
            if (packet.CalculateCrc() != packet.Crc)
            {
                errorCode = 1; //CRC Mismatch
                return new Packet();
            }
            else
            {
                return packet;
            }
        }

        public void WritePacket(byte[] buffer)
        {
            byte[] temp = Output(true);
            temp.CopyTo(buffer, 0);
        }

        private byte[] Output(bool includeCrc = true)
        {
            var size = Payload.Length;

            var fragmented = ((PacketType & PacketType.Fragmented) == PacketType.Fragmented);
            var reliableOrOrdered = ((PacketType & PacketType.Ordered) == PacketType.Ordered) ||
                                    ((PacketType & PacketType.Reliable) == PacketType.Reliable);

            if (reliableOrOrdered && fragmented)
                size += FRAGMENTED_RELIABLE_OR_ORDERED_HEADER_SIZE;
            else if (reliableOrOrdered)
                size += RELIABLE_OR_ORDERED_HEADER_SIZE;
            else if (fragmented)
                size += FRAGMENTED_HEADER_SIZE;


            if (!includeCrc)
                size--; //Remove CRC


            var buffer = new byte[size];
            using (var stream = new MemoryStream(buffer))
            {
                using (var writer = new BinaryWriter(stream))
                {
                    if (includeCrc)
                        writer.Write(Crc);
                    writer.Write(Sequence);
                    writer.Write((byte) PacketType);
                    if (reliableOrOrdered)
                    {
                        writer.Write(Acknowledgement);
                        writer.Write(AcknowledgementFlags);
                    }

                    if (fragmented)
                    {
                        writer.Write(FragmentId);
                        writer.Write(NumFragment);
                    }

                    writer.Write((short) Payload.Length);
                    writer.Write(Payload);
                }

                return stream.ToArray();
            }
        }


        //Applies the Calculated CRC
        public void GenerateCrc()
        {
            Crc = CalculateCrc();
        }

        //Does not apply the CRC only calculates
        public int CalculateCrc()
        {
            //Exclude Crc when calculating CRC
            return CalculateCrc(Output(false));
        }

        public static int CalculateCrc(byte[] buffer)
        {
            int crc = 0;
            for (var i = 0; i < Mathf.CeilToInt(buffer.Length / 4f); i++)
            {
                int chunk = 0;
                for (var j = 0; j < 4; j++)
                    chunk |= (buffer.Length <= 4 * i + j) ? 0 : buffer[4 * i + 1] << (32 - (j + 1) * 8);
                crc ^= chunk;
            }

            return crc;
        }
    }
    /// <summary>
    /// Specifies
    /// </summary>
//    public abstract class Packet
//    {
//        public int Checksum { get; private set; }
//        public short Sequence { get; private set; }
//        public abstract PacketType PacketType { get; }    
//        public byte[] Payload { get; private set; }  
//    }
//    
//
//    public enum PacketType : byte
//    {
//        Fragmented = (1 << 0),
//        Orderered = (1 << 1),
//        Reliable = (1 << 2)
//    }
//
//    public class FragmentedPacket : Packet
//    {
//        public override PacketType PacketType
//        {
//            get { return PacketType.Fragmented; }
//        }
//        public byte FragmentId { get; private set; }
//        public byte FragmentCount { get; private set; }
//    }
//
//    public class FragmentBuffer
//    {
//        private class FragmentCollection
//        {
//            private 
//        }
//        
//        public FragmentBuffer()
//        {
//            _buffer = new int[byte.MaxValue+1];
//        }
//        private int[] _buffer;
//       
//        
//    }
}