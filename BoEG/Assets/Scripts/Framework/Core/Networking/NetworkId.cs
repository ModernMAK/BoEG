using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace FrameWork.Core.Networking
{
    public class NetworkId : MonoBehaviour, INetworkedUnityObject<GameObject>
    {
        
        
        public bool Register(INetworkedDictionary<INetworkIdentifier> networkedDictionary)
        {
            if (Id == Guid.Empty)
                Id = networkedDictionary.RequestId();
            return networkedDictionary.TryAdd(this);
        }

        //Should make readonly, its only meant to be viewed
        [SerializeField] private string _debug;
        private Guid _guid;

        public Guid Id
        {
            get => _guid;
            set
            {
                _guid = value;
                _debug = value.ToString();
            }
        }
        Object INetworkedUnityObject.Object => Object;
        public GameObject Object => gameObject;

        [MenuItem("BoEG/Networking/Generate Scene NetIds")]
        public static void GenerateSceneNetworkIds()
        {
            var allNetIds = FindObjectsOfType<NetworkId>();
            foreach (var netId in allNetIds)
                if(netId.Id == Guid.Empty)
                    netId.GeneratedId();
        }
        
        private void GeneratedId() => Id = Guid.NewGuid();
    }
}