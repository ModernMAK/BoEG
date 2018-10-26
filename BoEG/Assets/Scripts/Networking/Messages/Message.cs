namespace Networking.Messages
{
    public class Message
    {
        public virtual MessageType MessageType
        {
            get { return MessageType.Error; }
        }
        public virtual byte[] Write()
        {
            return new byte[0];
        }

        public virtual void Read(byte[] data)
        {
        }
    }
}