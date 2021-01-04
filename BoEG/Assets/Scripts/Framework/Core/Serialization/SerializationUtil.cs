using System.Collections.Generic;

namespace MobaGame.Framework.Core.Serialization
{
    public static class SerializationUtil
    {
        public static void Serialize<T>(this IEnumerable<T> self, ISerializer serializer) where T : ISerializable
        {
            foreach (var serializable in self) serializable.Serialize(serializer);
        }

        public static void Serialize<T>(this ISerializer self, IEnumerable<T> serializable) where T : ISerializable
        {
            serializable.Serialize(self);
        }

        public static void Deserialize<T>(this IEnumerable<T> self, IDeserializer deserializer)
            where T : IDeserializable
        {
            foreach (var deserializable in self) deserializable.Deserialize(deserializer);
        }

        public static void Deserialize<T>(this IDeserializer self, IEnumerable<T> deserializable)
            where T : IDeserializable
        {
            deserializable.Deserialize(self);
        }
    }
}