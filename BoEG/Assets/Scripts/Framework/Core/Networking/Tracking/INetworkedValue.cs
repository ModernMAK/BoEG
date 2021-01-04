namespace MobaGame.Framework.Core.Networking.Tracking
{
    public interface INetworkedValue<out T> : INetworkIdentifier
    {
        public T Value { get; }
    }
}