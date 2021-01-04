using System.Collections.Generic;

namespace MobaGame.Framework.Core.Modules
{
    public interface IAggroable
    {
        float AggroRange { get; }
        IReadOnlyList<Actor> GetAggroTargets();
        Actor GetAggroTarget(int index);
        bool HasAggroTarget();
    }
}