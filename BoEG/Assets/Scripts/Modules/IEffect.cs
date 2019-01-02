using UnityEngine;

namespace Modules
{
    public interface IEffect
    {
        void Initialize(GameObject source, GameObject target);
        void PreTick();
        void Tick();
        void PostTick();
        void Terminate();
        bool ShouldTerminate { get; }
    }
}