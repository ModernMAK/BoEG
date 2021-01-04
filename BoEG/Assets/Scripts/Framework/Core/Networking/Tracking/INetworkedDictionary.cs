using System.Collections.Generic;

namespace MobaGame.Framework.Core.Networking.Tracking
{


    public interface INetworkedDictionary<TValue> : IReadOnlyDictionary<SerializableGuid, TValue> where TValue : INetworkIdentifier
    {
        public SerializableGuid RequestId();
        public bool TryAdd(TValue value);
        public bool TryRemove(TValue value);
        public bool ContainsKey(TValue value);
    }

}