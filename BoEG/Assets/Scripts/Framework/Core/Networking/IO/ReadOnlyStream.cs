using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace MobaGame.Framework.Core.Networking.IO
{
    public class WriteOnlyStream : NonClosingStream
    {
        private const string WriteOnlyError = "The Stream is Write-Only!";

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException(WriteOnlyError);
        }

        public override int Read(byte[] buffer, int offset, int count) =>
            throw new NotSupportedException(WriteOnlyError);

        public override int ReadByte() =>
            throw new NotSupportedException(WriteOnlyError);

        public override Task<int> ReadAsync(byte[] buffer, int offset, int count,
            CancellationToken cancellationToken) =>
            throw new NotSupportedException(WriteOnlyError);

        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback,
            object state) =>
            throw new NotSupportedException(WriteOnlyError);

        public override int EndRead(IAsyncResult asyncResult) =>
            throw new NotSupportedException(WriteOnlyError);

        public override void SetLength(long value)
        {
            if (value < UnderlyingStream.Length)
                throw new NotSupportedException(WriteOnlyError);
            base.SetLength(value);
        }

        public override long Position
        {
            get => base.Position;
            set => throw new NotSupportedException(WriteOnlyError);
        }

        public override bool CanRead => false;

        public WriteOnlyStream(Stream stream) : base(stream)
        {
        }
    }

    /// <summary>
    /// Wraps a stream so that write operations are disabled without recreating the steam.
    /// Does not close the underlying stream.
    /// </summary>
    public class ReadOnlyStream : NonClosingStream
    {
        private const string ReadOnlyError = "The Stream is Read-Only!";

        public ReadOnlyStream(Stream stream) : base(stream)
        {
        }


        public override void Flush()
        {
            //TODO figure out if this is a read only operation?
            throw new InvalidOperationException(ReadOnlyError);
        }

        public override void SetLength(long value)
        {
            throw new InvalidOperationException(ReadOnlyError);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new InvalidOperationException(ReadOnlyError);
        }

        public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            throw new InvalidOperationException(ReadOnlyError);
        }

        public override void WriteByte(byte value)
        {
            throw new InvalidOperationException(ReadOnlyError);
        }

        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback,
            object state)
        {
            throw new InvalidOperationException(ReadOnlyError);
        }

        public override void EndWrite(IAsyncResult asyncResult)
        {
            throw new InvalidOperationException(ReadOnlyError);
        }

        public override bool CanWrite => false;
    }
}