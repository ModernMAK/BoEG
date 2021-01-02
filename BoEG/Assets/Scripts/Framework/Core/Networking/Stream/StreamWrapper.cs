using System.IO;

namespace Framework.Core.Networking
{
    /// <summary>
    /// Stream Wrapper wraps a stream, exposing it as a base stream type.
    /// This class isn't intended to do anything special, so underlying types inherit this and perform their actions.
    ///
    /// It does 'unintentionally' prevent the underlying stream from closing.
    /// </summary>
    public abstract class StreamWrapper : Stream
    {
        private readonly Stream _stream;
        protected Stream UnderlyingStream => _stream;
        protected StreamWrapper(Stream stream)
        {
            _stream = stream;
        }

        public override void Flush() => _stream.Flush();

        public override int Read(byte[] buffer, int offset, int count) => _stream.Read(buffer,offset,count);

        public override long Seek(long offset, SeekOrigin origin) => _stream.Seek(offset ,origin);

        public override void SetLength(long value) => _stream.SetLength(value);

        public override void Write(byte[] buffer, int offset, int count) => _stream.Write(buffer, offset, count);

        public override bool CanRead => _stream.CanRead;

        public override bool CanSeek => _stream.CanSeek;

        public override bool CanWrite => _stream.CanWrite;

        public override long Length => _stream.Length;

        public override long Position
        {
            get => _stream.Position;
            set => _stream.Position = value;
        }
    }
}