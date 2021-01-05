using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MobaGame.Framework.Core.Networking.Tracking
{
    public abstract class NetworkDictionary<T> : MonoBehaviour, INetworkedDictionary<T> where T : INetworkIdentifier
    {
        private NetworkedDictionary<T> _networkedDictionary;

        protected virtual void Awake()
        {
            _networkedDictionary = new NetworkedDictionary<T>();
        }


        public IEnumerator<KeyValuePair<SerializableGuid, T>> GetEnumerator()
        {
            return _networkedDictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) _networkedDictionary).GetEnumerator();
        }

        public int Count => _networkedDictionary.Count;

        public bool ContainsKey(SerializableGuid key)
        {
            return _networkedDictionary.ContainsKey(key);
        }

        public bool TryGetValue(SerializableGuid key, out T value)
        {
            return _networkedDictionary.TryGetValue(key, out value);
        }

        public T this[SerializableGuid key] => _networkedDictionary[key];

        public IEnumerable<SerializableGuid> Keys => _networkedDictionary.Keys;

        public IEnumerable<T> Values => _networkedDictionary.Values;

        public SerializableGuid RequestId()
        {
            return _networkedDictionary.RequestId();
        }

        public bool TryAdd(T value)
        {
            return _networkedDictionary.TryAdd(value);
        }

        public bool TryRemove(T value)
        {
            return _networkedDictionary.TryRemove(value);
        }

        public bool ContainsKey(T value)
        {
            return _networkedDictionary.ContainsKey(value);
        }
    }
}