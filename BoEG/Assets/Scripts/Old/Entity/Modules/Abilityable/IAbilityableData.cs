using System.Collections.Generic;

namespace Old.Entity.Modules.Abilityable
{
    public interface IAbilityableData
    {
        IEnumerable<IAbilityData> Abilities { get; }
        int AbilityCount { get; }
    }
}