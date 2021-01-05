using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MobaGame.Framework.Core.Networking.IO;
using MobaGame.Framework.Core.Serialization;
using UnityEngine;

namespace MobaGame.Framework.Core.Networking.Tracking
{
    [RequireComponent(typeof(NetworkId))]
    public class NetworkSerializer : MonoBehaviour
    {
        [SerializeField] private NetworkId _networkId;

        private void Awake()
        {
            _networkId = GetComponent<NetworkId>();
        }

        private IEnumerable<Tuple<SerializableGuid, INetworkedSerializable>> GetAllSerializable()
        {
            foreach (var kvp in _networkId.Components)
            {
                var guid = kvp.Key;
                var networked = kvp.Value;

                if (networked is INetworkedSerializable serializable)
                    yield return new Tuple<SerializableGuid, INetworkedSerializable>(guid, serializable);
            }
        }

        public static bool TryGetSerializable<T>(INetworkedDictionary<T> dicitonary, SerializableGuid guid,
            out INetworkedSerializable serializable) where T : INetworkIdentifier
        {
            serializable = default;
            if (!dicitonary.TryGetValue(guid, out var networked)) return false;
            if (!(networked is INetworkedSerializable networkedSerializable)) return false;
            serializable = networkedSerializable;
            return true;
        }

        const long TempLength = 0L;

        public static bool Serialize(WriteOnlyStream writeOnly, Stream stream, SerializableGuid guid,
            INetworkedSerializable serializable, BinarySerializer serializer)
        {
            var originPos = stream.Position;
            //Write GUID
            serializer.Write(guid.ToByteArray());
            //Write temporary length
            var lengthPos = stream.Position;
            serializer.Write(TempLength);
            //'Optional' information
            var startPos = stream.Position;
            var serialized = serializable.Serialize(writeOnly);
            if (!serialized)
            {
                stream.Position = originPos;
                // stream.SetLength(originPos); We trim excess content at in a seperate functino
                return false;
            }

            var endPos = stream.Position;

            //Write Length
            var length = endPos - startPos;
            stream.Position = lengthPos;
            serializer.Write(length);

            //Return To Stream 
            stream.Position = endPos;
            return true;
        }

        public static bool Deserialize(ReadOnlyStream readOnly, Stream stream,
            INetworkedDictionary<INetworkedUnityObject<Component>> lookup, BinaryDeserializer deserializer)
        {
            var originPos = stream.Position;
            //Read GUID
            deserializer.ReadBytes(GuidBuffer, 0, 16);
            var guid = new Guid(GuidBuffer); //Auto converts to serializable when needed, so we just use guid here
            //Read length
            var length = deserializer.ReadLong();
            //On failure, report deserialization failure & Advance stream
            if (!TryGetSerializable(lookup, guid, out var serializable))
            {
                stream.Position += length;
                return false;
            }

            //Specify 'Range'
            var startPos = stream.Position;
            var endPos = stream.Position + length;
            //Perform Deserialization
            serializable.Deserialize(readOnly);
            //Validate
            if (stream.Position != endPos)
            {
                UnityEngine.Debug.Log("WARNING!"); //TODO
                stream.Position = endPos;
                return true; //Consider returning false?
            }

            return true;
        }

        private static readonly byte[] GuidBuffer = new byte[16];

        public static bool Deserialize(ReadOnlyStream readOnly, Stream stream,
            IReadOnlyDictionary<SerializableGuid, INetworkedUnityObject<NetworkId>> lookup,
            BinaryDeserializer deserializer, out int failures)
        {
            failures = 0;
            //Read GUID (16 bytes)
            deserializer.ReadBytes(GuidBuffer, 0, 16);
            var guid = new SerializableGuid(new Guid(GuidBuffer));
            //Read Length
            var length = deserializer.ReadLong();
            var read = 0L;
            //try find
            if (!lookup.TryGetValue(guid, out var netIdObj))
            {
                //Skip section if not found
                stream.Position += length;

                UnityEngine.Debug.Log("A NetworkID is missing!"); //TODO
                failures = int.MaxValue;
                return false;
            }
            //READ
            var netId = netIdObj.Object;
            var startPos = stream.Position;
            while (read < length)
            {
                var startRead = stream.Position;
                var failed = Deserialize(readOnly, stream, netId.Components, deserializer);
                var endRead = stream.Position;
                read += (endRead - startRead);
                failures += failed ? 1 : 0;
            }
            //Validate
            if (read != length)
            {
                UnityEngine.Debug.Log("WARNING!"); //TODO
                stream.Position = startPos + length;
                //Technically not a failure
            }
            return true;
        }

        public static bool Deserialize(Stream stream, INetworkedDictionary<INetworkedUnityObject<Component>> lookup)
        {
            var readOnly = new ReadOnlyStream(stream);
            using (var reader = new BinaryReader(readOnly, Encoding.UTF8))
            {
                var deserializer = new BinaryDeserializer(reader);
                return Deserialize(readOnly, stream, lookup, deserializer);
            }

        }

        public static bool Serialize(Stream stream, SerializableGuid id,
            IEnumerable<Tuple<SerializableGuid, INetworkedSerializable>> serializables)
        {
            var written = false;
            var originPos = stream.Position;
            var writeOnly = new WriteOnlyStream(stream);
            using (var writer = new BinaryWriter(writeOnly, Encoding.UTF8))
            {
                var serializer = new BinarySerializer(writer);

                //Write GUID
                serializer.Write(id.ToByteArray());

                //Write temporary length
                var lengthPos = stream.Position;
                serializer.Write(TempLength);

                //Write optional information
                var startPos = stream.Position;
                foreach (var (subId, serializable) in serializables)
                {
                    written |= Serialize(writeOnly, stream, subId, serializable, serializer);
                }

                var endPos = stream.Position;
                //If nothing written, reset stream and trim content
                if (!written)
                {
                    stream.Position = originPos;
                    stream.SetLength(originPos);
                    return false;
                }

                //Write Length
                var length = endPos - startPos;
                stream.Position = lengthPos;
                serializer.Write(length);

                //Return To Stream 
                stream.Position = endPos;

                return true;
            }
        }

        public bool Serialize(Stream stream)
        {
            return Serialize(stream, _networkId.Id, GetAllSerializable());
        }

        public bool Deserialize(Stream stream)
        {
            return Deserialize(stream, _networkId.Components);
        }
    }
}