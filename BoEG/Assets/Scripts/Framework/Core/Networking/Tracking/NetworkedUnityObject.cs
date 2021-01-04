using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MobaGame.Framework.Core.Networking.Tracking
{
    [Serializable]
    public class NetworkedUnityObject<TObject> : INetworkedUnityObject<TObject> where TObject : Object
    {
        [SerializeField] private SerializableGuid _guid;
        private readonly TObject _object;
        

        public NetworkedUnityObject(TObject obj) : this(obj, Guid.Empty)
        {
        }

        public NetworkedUnityObject(TObject obj, Guid guid)
        {
            _guid = guid;
            _object = obj;
        }

        public SerializableGuid Id
        {
            get => _guid;
            set => _guid = value;
        }

        public void SetId(SerializableGuid id)
        {
            if(_guid == Guid.Empty)
                _guid = id;
            else if (id == Guid.Empty)
                _guid = id;
            else throw new InvalidOperationException();
        }

        Object INetworkedUnityObject.Object => Object;

        public TObject Object => _object;
    }
}