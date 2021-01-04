using System;
using System.Collections.Generic;

namespace MobaGame.Framework.Core.Networking.Tracking
{
    public interface INetworkedDictionary<T> : IReadOnlyDictionary<Guid, T> where T : INetworkIdentifier
    {
        public Guid RequestId();
        public bool TryAdd(T value);
        public bool TryRemove(T value);

        public bool ContainsKey(T value);
    }
}