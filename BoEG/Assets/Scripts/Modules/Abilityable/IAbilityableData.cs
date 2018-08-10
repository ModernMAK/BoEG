using System.Collections.Generic;

namespace Modules.Abilityable
{
    public interface IAbilityableData
    {
        IEnumerable<IAbilityData> Abilities { get; }
        int AbilityCount { get; }
    }
}