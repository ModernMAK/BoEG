using Entity;
using UnityEngine;

namespace Core.OrderSystem
{
    public interface IJob : IStepable
    {
        void Initialize(GameObject go);
        void Terminate();
        bool IsDone();
    }
}