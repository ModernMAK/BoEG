using System;
using System.IO;

namespace Framework.Core.Networking
{
    public class ReadOnlyStream : Stream
    {
        private const string ReadOnlyError = "The Stream is Read-Only!";

        public ReadOnlyStream(Stream stream)
        {
            _stream = stream;
        }


        private readonly Stream _stream;


        public override void Flush()
        {
            //TODO figure out if this is a read only operation?
            _stream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return _stream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return _stream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            throw new InvalidOperationException(ReadOnlyError);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new InvalidOperationException(ReadOnlyError);
        }

        public override bool CanRead => _stream.CanRead;

        public override bool CanSeek => _stream.CanSeek;
        public override bool CanWrite => false;

        public override long Length => _stream.Length;

        public override long Position
        {
            get => _stream.Position;
            set => _stream.Position = value;
        }
    }
}