using System.Collections.Generic;
using UnityEngine;

namespace Framework.Core.Modules
{
    public interface IAggroable
    {
        float AggroRange { get; }
        IReadOnlyList<Actor> GetAggroTargets();
        Actor GetAggroTarget(int index);
        bool HasAggroTarget();
    }
}