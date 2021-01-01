namespace Framework.Core.Networking.MLAPI
{
    public abstract class AbstractMessageLayer
    {
        public MessageEventList Sent { get; }
        public MessageEventList Received { get; }

    }
}