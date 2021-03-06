using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MobaGame.Framework.Core.Networking.Tracking
{
    public class NetworkSceneManager : MonoBehaviour, INetworkedDictionary<INetworkedUnityObject>
    {
        private NetworkedDictionary<INetworkedUnityObject> _networkedDictionary;

        public void Awake()
        {
            _networkedDictionary = new NetworkedDictionary<INetworkedUnityObject>();
        }

        public IEnumerator<KeyValuePair<Guid, INetworkedUnityObject>> GetEnumerator()
        {
            return _networkedDictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) _networkedDictionary).GetEnumerator();
        }

        public int Count => _networkedDictionary.Count;

        public bool ContainsKey(Guid key)
        {
            return _networkedDictionary.ContainsKey(key);
        }

        public bool TryGetValue(Guid key, out INetworkedUnityObject value)
        {
            return _networkedDictionary.TryGetValue(key, out value);
        }

        public INetworkedUnityObject this[Guid key] => _networkedDictionary[key];

        public IEnumerable<Guid> Keys => _networkedDictionary.Keys;

        public IEnumerable<INetworkedUnityObject> Values => _networkedDictionary.Values;

        public Guid RequestId()
        {
            return _networkedDictionary.RequestId();
        }

        public bool TryAdd(INetworkedUnityObject value)
        {
            return _networkedDictionary.TryAdd(value);
        }

        public bool TryRemove(INetworkedUnityObject value)
        {
            return _networkedDictionary.TryRemove(value);
        }

        public bool ContainsKey(INetworkedUnityObject value)
        {
            return _networkedDictionary.ContainsKey(value);
        }
    }
}