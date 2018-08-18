using System.Collections.Generic;

namespace Core.Serialization
{
    public static class SerializatoinUtil{
        public static void Serialize<T>(this IEnumerable<T> self, ISerializer serializer) where T : ISerializable
        {
            foreach (var serializable in self)
            {
                serializable.Serialize(serializer);
            }
        }
        public static void Deserialize<T>(this IEnumerable<T> self, IDeserializer deserializer) where T : IDeserializable
        {
            foreach (var deserializable in self)
            {
                deserializable.Deserialize(deserializer);
            }
        }
        
    }
}