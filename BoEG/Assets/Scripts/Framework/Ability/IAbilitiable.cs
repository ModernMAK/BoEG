using System;
using Entity.Abilities.FlameWitch;
using Framework.Core.Modules;

namespace Framework.Ability
{
    public interface IAbilitiable
    {
        int AbilityCount { get; }
        bool FindAbility<T>(out T ability);
        IAbility GetAbility(int index);
        event EventHandler<SpellEventArgs> SpellCasted;

        void NotifySpellCast(SpellEventArgs e);
    }
}