using Entity;
using UnityEngine;

namespace Core.OrderSystem
{
    public interface IJob : ITickable
    {
        void Initialize(GameObject go);
        void Terminate();
        bool IsDone();
    }
}