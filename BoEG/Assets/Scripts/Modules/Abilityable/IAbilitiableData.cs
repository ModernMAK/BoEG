using System.Collections.Generic;
using Modules.Abilityable.Ability;

namespace Modules.Abilityable
{
    public interface IAbilitiableData
    {
        IEnumerable<IAbilityData> Abilities { get; }
        int AbilityCount { get; }
    }
}