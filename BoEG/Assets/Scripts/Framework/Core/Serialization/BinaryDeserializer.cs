using System.IO;

namespace MobaGame.Framework.Core.Serialization
{
    public class BinaryDeserializer : IDeserializer
    {
        private readonly BinaryReader _reader;

        public BinaryDeserializer(BinaryReader reader)
        {
            _reader = reader;
        }


        public bool ReadBool()
        {
            return _reader.ReadBoolean();
        }

        public byte ReadByte()
        {
            return _reader.ReadByte();
        }

        public sbyte ReadSByte()
        {
            return _reader.ReadSByte();
        }

        public byte[] ReadBytes(int len)
        {
            return _reader.ReadBytes(len);
        }

        public int ReadBytes(byte[] buffer, int offset, int len)
        {
            return _reader.Read(buffer, offset, len);
        }

        public char ReadChar()
        {
            return _reader.ReadChar();
        }

        public char[] ReadChars(int len)
        {
            return _reader.ReadChars(len);
        }

        public string ReadString()
        {
            return _reader.ReadString();
        }


        public float ReadFloat()
        {
            return _reader.ReadSingle();
        }

        public double ReadDouble()
        {
            return _reader.ReadDouble();
        }

        public decimal ReadDecimal()
        {
            return _reader.ReadDecimal();
        }


        public short ReadShort()
        {
            return _reader.ReadInt16();
        }

        public ushort ReadUShort()
        {
            return _reader.ReadUInt16();
        }

        public int ReadInt()
        {
            return _reader.ReadInt32();
        }

        public uint ReadUInt()
        {
            return _reader.ReadUInt32();
        }

        public long ReadLong()
        {
            return _reader.ReadInt64();
        }

        public ulong ReadULong()
        {
            return _reader.ReadUInt64();
        }
    }
}