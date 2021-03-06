namespace MobaGame.Framework.Core.Networking.StreamTypes
{
    /// <summary>
    /// Wraps a stream.
    /// This stream does not close the underlying stream.
    /// </summary>
    public class NonClosingStream : StreamWrapper
    {
        public NonClosingStream(System.IO.Stream stream) : base(stream)
        {
        }
    }
}