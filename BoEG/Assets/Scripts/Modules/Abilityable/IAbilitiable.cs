using System.Collections.Generic;
using Modules.Abilityable.Ability;

namespace Modules.Abilityable
{
    public interface IAbilitiable
    {
        void Cast(int index);
        void LevelUp(int index);
        T GetAbility<T>() where T : IAbility;
        IEnumerable<IAbility> Abilities { get; }
    }
}