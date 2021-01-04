using UnityEngine;

namespace MobaGame.Framework.Core.Networking.Tracking
{
    public interface INetworkedUnityObject<out T> : INetworkedUnityObject where T : Object
    {
        public new T Object { get; }
    }

    public interface INetworkedUnityObject : INetworkIdentifier
    {
        public Object Object { get; }
    }
}