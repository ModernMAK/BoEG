using System;
using System.IO;
using Framework.Core.Serialization;

namespace Framework.Core.Networking.MLAPI
{
    public class Message : ISerializable, IDeserializable, IDisposable
    {
        private const int BufferSize = 4096;
        private static readonly byte[] Buffer = new byte[BufferSize];

        public Message()
        {
            Code = default;
            InternalStream = new MemoryStream();
            Stream = new ReadOnlyStream(InternalStream);
        }

        public Message(int code, Stream stream)
        {
            Code = code;
            InternalStream = null;
            Stream = new ReadOnlyStream(stream);
        }

        public Message(int code, ReadOnlyStream stream)
        {
            Code = code;
            InternalStream = null;
            Stream = stream;
        }

        public int Code { get; private set; }

        private Stream InternalStream { get; }
        public ReadOnlyStream Stream { get; }
        private bool OwnStream => InternalStream != null;

        public void Serialize(ISerializer serializer)
        {
            var stream = Stream;
            serializer.Write(Code);
            serializer.Write(Stream.Length); //LONG not int
            stream.Seek(0, SeekOrigin.Begin);
            while (stream.Position != stream.Length)
            {
                var read = stream.Read(Buffer, 0, BufferSize);
                serializer.Write(Buffer, 0, read);
            }
        }

        public void Deserialize(IDeserializer deserializer)
        {
            var stream = InternalStream;
            Code = deserializer.ReadInt();
            var len = deserializer.ReadLong();
            stream.SetLength(len);
            stream.Seek(0, SeekOrigin.Begin);
            while (stream.Position != stream.Length)
            {
                var read = deserializer.ReadBytes(Buffer, 0, BufferSize);
                stream.Write(Buffer, 0, read);
            }

            stream.Seek(0, SeekOrigin.Begin);
        }

        public void Dispose()
        {
            if (OwnStream)
                Stream?.Dispose();
        }
    }
}