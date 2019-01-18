using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Core.Modules
{
    public interface IAggroable
    {
        float AggroRange { get; }
        bool InAggro(GameObject go);
        IEnumerable<GameObject> GetAggroTargets();
        bool HasAggroTarget();
    }
}