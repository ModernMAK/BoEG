using Core.Serialization;
using Entity;

namespace Modules
{
    public interface IModule : ITickable, ISerializable, IDeserializable
    {
        void Initialize();
        void Terminate();
//        void PreTick();
//        void Tick();
//        void PostTick();
    }
}