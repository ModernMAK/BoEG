using UnityEngine;

namespace Modules
{
    public interface IEffect
    {
        bool ShouldTerminate { get; }
        void Initialize(GameObject source, GameObject target);
        void PreTick();
        void Tick();
        void PostTick();
        void Terminate();
    }
}