namespace MobaGame.Framework.Core.Networking.Tracking
{
    public interface INetworkIdentifier
    {
        public SerializableGuid Id { get; }
        public void SetId(SerializableGuid id);

    }
}