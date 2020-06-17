using System.Collections.Generic;
using UnityEngine;

namespace Framework.Core.Modules
{
    public interface IAggroable
    {
        float AggroRange { get; }
        bool InAggro(GameObject go);
        IReadOnlyList<GameObject> GetAggroTargets();
        GameObject GetAggroTarget(int index);
        bool HasAggroTarget();
    }
}