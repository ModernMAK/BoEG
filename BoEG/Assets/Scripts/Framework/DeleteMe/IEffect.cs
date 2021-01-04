using UnityEngine;

namespace MobaGame.Framework.DeleteMe
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