using UnityEngine;

namespace Framework.Core.Modules
{
    public interface IAggroable
    {
        float AggroRange { get; }
        bool InAggro(GameObject go);
    }
}