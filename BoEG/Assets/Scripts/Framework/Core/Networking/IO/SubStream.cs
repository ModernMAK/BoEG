using System;
using System.IO;

namespace MobaGame.Framework.Core.Networking.IO
{
    /// <summary>
    /// Wraps a stream so that operations are local to a section of the stream.
    /// This does not close the parent stream on termination
    /// </summary>
    public class SubStream : NonClosingStream
    {
        public SubStream(Stream stream, long start, long end) : base(stream)
        {
            _start = start;
            _end = end;
            if(_start > _end)
                throw new ArgumentException("Start cannot be greater than End");
        }

        private readonly long _start;
        private readonly long _end;

        public override long Length => _end - _start;


        private void AssertValidPosition(long position)
        {
            if (_start > position || position > _end)
                throw new InvalidOperationException("Substream does not have access to items outside the substream!");
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            long newPos;
            switch (origin)
            {
                case SeekOrigin.Begin:
                    newPos = _start + offset;
                    break;
                case SeekOrigin.Current:
                    newPos = Position + offset;
                    break;
                case SeekOrigin.End:
                    newPos = _end + offset;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(origin), origin, null);
            }

            AssertValidPosition(newPos);
            base.Seek(newPos, SeekOrigin.Begin);
            return newPos - _start;
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException("Sub Streams cannot alter the parent stream's length");
        }

        public override long Position
        {
            get => base.Position - _start;
            set
            {
                var position = value + _start;
                AssertValidPosition(position);
                base.Position = position;
            }
        }
    }
}