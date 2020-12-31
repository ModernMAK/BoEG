namespace FrameWork.Core.Networking
{
    public interface INetworkedValue<out T> : INetworkIdentifier
    {
        public T Value { get; }
    }
}