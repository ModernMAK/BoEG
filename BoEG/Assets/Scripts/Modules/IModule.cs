using Entity;

namespace Modules
{
    public interface IModule : ITickable
    {
        void Initialize();
        void Terminate();
//        void PreTick();
//        void Tick();
//        void PostTick();
    }
}