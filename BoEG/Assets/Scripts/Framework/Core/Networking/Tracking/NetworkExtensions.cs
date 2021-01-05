using System;
using MobaGame.Framework.Core.Networking.IO;

namespace MobaGame.Framework.Core.Networking.Tracking
{
    public static class NetworkExtensions
    {
        public static void GenerateId<T>(this T identifier)
            where T : INetworkIdentifier
            => identifier.SetId(Guid.NewGuid());

        public static void GenerateId<T>(this T identifier, INetworkedDictionary<T> dictionary)
            where T : INetworkIdentifier
            => identifier.SetId(dictionary.RequestId());

        public static void ClearId<T>(this T identifier)
            where T : INetworkIdentifier
            => identifier.SetId(Guid.Empty);

        public static bool TryAdd<T>(this T identifier, INetworkedDictionary<T> dictionary)
            where T : INetworkIdentifier
            => dictionary.TryAdd(identifier);


        public static bool TryRemove<T>(this T identifier, INetworkedDictionary<T> dictionary)
            where T : INetworkIdentifier
            => dictionary.TryRemove(identifier);

        public static bool TryGetAs<T,U>(this INetworkedDictionary<T> dictionary, SerializableGuid guid, out U result)
            where T : INetworkIdentifier
        {
            result = default;
            if (!dictionary.TryGetValue(guid, out var data)) return false;
            if (data is U value)
            {
                result = value;
                return true;
            }

            return false;

        }

        public static bool TrySerialize<T>(this INetworkedDictionary<T> dictionary, SerializableGuid guid, WriteOnlyStream stream) where T : INetworkIdentifier
        {
            if (!dictionary.TryGetAs<T, INetworkedSerializable>(guid, out var result)) return false;
            result.Serialize(stream);
            return true;
        }
        public static bool TryDeserialize<T>(this INetworkedDictionary<T> dictionary, SerializableGuid guid, ReadOnlyStream stream) where T : INetworkIdentifier
        {
            if (!dictionary.TryGetAs<T, INetworkedSerializable>(guid, out var result)) return false;
            result.Deserialize(stream);
            return true;
        }
    }
}