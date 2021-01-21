using System;
using MobaGame.Framework.Core.Modules.Ability;

namespace MobaGame.Framework.Core.Modules
{
    public interface IAbilitiable
    {
        int AbilityCount { get; }
        bool TryGetAbility<T>(out T ability);
        IAbility GetAbility(int index);
        event EventHandler<SpellEventArgs> SpellCasted;
        event EventHandler AbilitiesChanged;

        void NotifySpellCast(SpellEventArgs e);
    }
}