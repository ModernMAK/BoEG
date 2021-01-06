using System;
using UnityEditor;
using UnityEditor.SearchService;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MobaGame.Framework.Core.Networking.Tracking
{
    public class NetworkId : MonoBehaviour, INetworkedUnityObject<NetworkId>
    {
       
        
        [SerializeField] private bool _sceneObject;
        public bool ServerObject => true;
        public bool SceneObject => _sceneObject;
        [SerializeField] private NetworkedUnityObject<NetworkId> _netObject;
        private NetworkedDictionary<INetworkedUnityObject<Component>> _components;
        public NetworkedDictionary<INetworkedUnityObject<Component>> Components => _components;
        public SerializableGuid Id => _netObject.Id;
        public void SetId(SerializableGuid id) => _netObject.SetId(id);

        public NetworkSceneObjectManager ObjectManager => NetworkSceneObjectManager.Instance;

        Object INetworkedUnityObject.Object => _netObject.Object;
        public NetworkId Object => _netObject.Object;

        MessageManager Manager => MessageManager.Instance;

        public bool Owned
        {
            get
            {
                if (ServerObject || SceneObject)
                    return Manager.IsServer;
                else
                    return Manager.IsClient;
            }
        }


        private void Awake()
        {
            _components = new NetworkedDictionary<INetworkedUnityObject<Component>>();
            _netObject = new NetworkedUnityObject<NetworkId>(this);
            if (SceneObject)
                NetworkSceneObjectManager.Instance.TryAdd(this);
        }

#if UNITY_EDITOR
        [MenuItem("BoEG/Networking/Generate Scene NetIds")]
        public static void GenerateSceneNetworkIds()
        {
            var allNetIds = FindObjectsOfType<NetworkId>();
            foreach (var netId in allNetIds)
                if (netId.Id == Guid.Empty)
                {
                    netId.GenerateId();
                    netId._sceneObject = true;
                }
        }
#endif
    }
}