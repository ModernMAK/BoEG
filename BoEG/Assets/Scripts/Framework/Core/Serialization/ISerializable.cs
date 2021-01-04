namespace MobaGame.Framework.Core.Serialization
{
    public interface ISerializable
    {
        // bool ShouldSerialize{ get; }
        void Serialize(ISerializer serializer);
        // void Deserializer(IDeserializer deserializer);
    }
}