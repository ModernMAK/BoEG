using System.IO;
using MobaGame.Framework.Core.Networking.IO;
using MobaGame.Framework.Core.Networking.Tracking;
using MobaGame.Framework.Core.Serialization;
using MobaGame.Framework.Types;
using UnityEngine;

namespace MobaGame.Framework.Core.Modules
{
    // [DisallowMultipleComponent]
    public class NetworkedHealthable : Healthable, INetworkedUnityObject<Component>, INetworkedSerializable
    {
        private NetworkId _networkId;

        protected void Awake()
        {
            _networkId = GetComponent<NetworkId>();
            _networkedSelf = new NetworkedUnityObject<Component>(this);
        }

        public bool Owned => _networkId.Owned;

        public override void Register(IStepableEvent source)
        {
            if (Owned)
                base.Register(source);
        }
        public override void Unregister(IStepableEvent source)
        {
            if (Owned)
                base.Unregister(source);
        }

        private NetworkedUnityObject<Component> _networkedSelf;
        public SerializableGuid Id => _networkedSelf.Id;

        public void SetId(SerializableGuid id)
        {
            _networkedSelf.SetId(id);
        }

        Object INetworkedUnityObject.Object => ((INetworkedUnityObject) _networkedSelf).Object;

        public Component Object => _networkedSelf.Object;


        public bool Serialize(WriteOnlyStream stream)
        {
            using var writer = new BinaryWriter(stream);
            var serializer = new BinarySerializer(writer);
            serializer.Write(RawStatCapacity);
            serializer.Write(RawStatPercentage);
            serializer.Write(RawStatGeneration);
            return true;
        }

        public bool Deserialize(ReadOnlyStream stream)
        {
            using var reader = new BinaryReader(stream);
            var deserializer = new BinaryDeserializer(reader);
            RawStatCapacity = deserializer.ReadFloat();
            RawStatPercentage = deserializer.ReadFloat();
            RawStatGeneration = deserializer.ReadFloat();
            return true;
        }
    }
}