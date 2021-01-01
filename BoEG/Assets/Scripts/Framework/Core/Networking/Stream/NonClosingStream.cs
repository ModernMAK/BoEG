using System.IO;

namespace Framework.Core.Networking
{
    /// <summary>
    /// Wraps a stream.
    /// This stream does not close the underlying stream.
    /// </summary>
    public class NonClosingStream : StreamWrapper
    {
        public NonClosingStream(Stream stream) : base(stream)
        {
        }
    }
}