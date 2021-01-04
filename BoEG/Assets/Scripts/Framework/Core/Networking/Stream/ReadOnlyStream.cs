using System;

namespace MobaGame.Framework.Core.Networking.Stream
{
    /// <summary>
    /// Wraps a stream so that write operations are disabled without recreating the steam.
    /// Does not close the underlying stream.
    /// </summary>
    public class ReadOnlyStream : NonClosingStream
    {
        private const string ReadOnlyError = "The Stream is Read-Only!";

        public ReadOnlyStream(System.IO.Stream stream) : base(stream)
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
        public override bool CanWrite => false;
    }
}