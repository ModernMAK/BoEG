using System.Collections.Generic;

namespace Components.Abilityable
{
    public interface IAbilityableData
    {
        IEnumerable<IAbilityData> Abilities { get; }
        int AbilityCount { get; }
    }
}