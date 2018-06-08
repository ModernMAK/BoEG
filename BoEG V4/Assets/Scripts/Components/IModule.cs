using Core;
using UnityEngine.Networking;

namespace Components
{
    public interface IModule
    {
        void Initialize(Entity e);

        void PreTick();

        void Tick();

        void PostTick();


        bool IsDirty();
        bool Serialize(NetworkWriter writer, bool initState = true);
        void Deserialize(NetworkReader reader, bool initState = true);
    }
}