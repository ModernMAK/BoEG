using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Configuration;

namespace FrameWork.Core.Networking
{
    public class NetworkedDictionary<T> : INetworkedDictionary<T> where T : INetworkIdentifier
    {
        private readonly Dictionary<Guid, T> _networked;

        public NetworkedDictionary() : this(new Dictionary<Guid, T>())
        {
        }

        public NetworkedDictionary(Dictionary<Guid, T> networked)
        {
            _networked = networked;
        }

        public IEnumerator<KeyValuePair<Guid, T>> GetEnumerator() => _networked.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int Count => _networked.Count;

        public bool ContainsKey(Guid key) => _networked.ContainsKey(key);
        public bool ContainsKey(T value) => _networked.ContainsKey(value.Id);

        public bool TryGetValue(Guid key, out T value) => _networked.TryGetValue(key, out value);

        public T this[Guid key] => _networked[key];

        public IEnumerable<Guid> Keys => _networked.Keys;

        public IEnumerable<T> Values => _networked.Values;

        public Guid RequestId()
        {
            return Guid.NewGuid();
        }

        public bool TryAdd(T value)
        {
            if (ContainsKey(value.Id))
                return false;
            _networked.Add(value.Id, value);
            return true;
        }

        public bool TryRemove(T value)
        {
            if (!TryGetValue(value.Id, out var current))
                return false;
            if (!current.Equals(value))
                return false;
            return _networked.Remove(value.Id);
        }
    }
}