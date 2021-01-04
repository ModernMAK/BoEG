using System.Collections;
using System.Collections.Generic;

namespace MobaGame.Framework.Core.Networking.Tracking
{
    public class NetworkedDictionary<T> : INetworkedDictionary<T> where T : INetworkIdentifier
    {
        private readonly Dictionary<SerializableGuid, T> _networked;

        public NetworkedDictionary() : this(new Dictionary<SerializableGuid, T>())
        {
        }

        public NetworkedDictionary(Dictionary<SerializableGuid, T> networked)
        {
            _networked = networked;
        }

        public IEnumerator<KeyValuePair<SerializableGuid, T>> GetEnumerator() => _networked.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int Count => _networked.Count;

        public bool ContainsKey(SerializableGuid key) => _networked.ContainsKey(key);
        public bool ContainsKey(T value) => _networked.ContainsKey(value.Id);

        public bool TryGetValue(SerializableGuid key, out T value) => _networked.TryGetValue(key, out value);

        public T this[SerializableGuid key] => _networked[key];

        public IEnumerable<SerializableGuid> Keys => _networked.Keys;

        public IEnumerable<T> Values => _networked.Values;

        public SerializableGuid RequestId()
        {
            return SerializableGuid.NewGuid();
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