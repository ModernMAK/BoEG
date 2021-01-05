namespace MobaGame.Framework.Core.Networking.Tracking
{
    public class NetworkSceneObjectManager : NetworkDictionary<INetworkedUnityObject<NetworkId>>
    {
        private static NetworkSceneObjectManager _instance;
        private const string SingletonName = NetworkSingletonNames.SceneObjectManager;

        public static NetworkSceneObjectManager Instance =>
            SingletonUtility.GetInstance(ref _instance, SingletonName, NetworkSingletonNames.SingletonGroup);

        protected override void Awake()
        {
            base.Awake();

            if (_instance == null)
                _instance = this;
            else
                UnityEngine.Debug.Log($"Multiple {SingletonName}!");
        }
    }
}