
namespace Framework.Core.Serialization
{
	public interface IDeserializable
	{
		// bool ShouldSerialize{ get; }
		//void Serialize(ISerializer serializer);
		void Deserialize(IDeserializer deserializer);
	
	}
}