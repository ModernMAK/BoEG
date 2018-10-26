using System.Collections.Generic;

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