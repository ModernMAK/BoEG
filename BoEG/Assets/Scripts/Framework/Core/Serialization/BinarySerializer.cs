using System;
using System.Collections;
using System.IO;

namespace Framework.Core.Serialization
{
    public class BinarySerializer : ISerializer
    {
        private readonly BinaryWriter _writer;

        public BinarySerializer(BinaryWriter writer)
        {
            _writer = writer;
        }

        public void Write(bool value)
        {
            _writer.Write(value);
            // Write(BitConverter.GetBytes(value)); //WHY? to ensure the boolean only takes a bit?
        }
        
        public void Write(byte value)
        {
            _writer.Write(value);
        }

        public void Write(sbyte value)
        {
            _writer.Write(value);
        }

        public void Write(byte[] value)
        {
            _writer.Write(value);
        }

        public void Write(byte[] value, int start, int len)
        {
            _writer.Write(value, start, len);
        }

        public void Write(char value)
        {
            _writer.Write(value);
        }

        public void Write(char[] value)
        {
            _writer.Write(value);
        }

        public void Write(char[] value, int start, int len)
        {
            _writer.Write(value, start, len);
        }

        public void Write(string value)
        {
            _writer.Write(value);
        }

        public void Write(short value)
        {
            _writer.Write(value);
        }

        public void Write(ushort value)
        {
            _writer.Write(value);
        }

        public void Write(float value)
        {
            _writer.Write(value);
        }

        public void Write(int value)
        {
            _writer.Write(value);
        }

        public void Write(uint value)
        {
            _writer.Write(value);
        }

        public void Write(double value)
        {
            _writer.Write(value);
        }

        public void Write(long value)
        {
            _writer.Write(value);
        }

        public void Write(ulong value)
        {
            _writer.Write(value);
        }

        public void Write(decimal value)
        {
            _writer.Write(value);
        }

    }
}