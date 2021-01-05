using MobaGame.Framework.Core.Networking.MLAPI;
using UnityEngine;

namespace MobaGame.Framework.Core.Networking.Tracking
{
    public class NetworkManager : MonoBehaviour
    {
        private static NetworkManager _instance;
        private const string SingletonName = NetworkSingletonNames.NetManager;

        private MessageClient _client;
        private MessageServer _server;
        protected MessageClient MessageClient => _client;
        protected MessageServer MessageServer => _server;

        public static NetworkManager Instance =>
            SingletonUtility.GetInstance(ref _instance, SingletonName, NetworkSingletonNames.SingletonGroup);

        protected void Awake()
        {
            if (_instance == null)
                _instance = this;
            else
                UnityEngine.Debug.Log($"Multiple {SingletonName}!");
        }

        public bool IsServer => MessageServer != null;
        public bool IsClient => MessageClient != null;

        // =======
        // A note here for later; this still exists, despite being obsolete
        // This was removed in 2018, and no longer functions
        // private void OnServerInitialized() {}
        // private void OnConnectedToServer() {}
        // ===
        // I mostly wanted to mention this because while rewriting mine, i keep finding remnants of the old stuff and go
        // 'wait is that still there?' I can safely say; its there, but just dont use it.
        // =======
    }
}