using System;

namespace MobaGame.Framework.Core.Modules
{
    public interface ITargetable
    {
        bool AllowAttackTargets { get; }
        bool AllowSpellTargets { get; }
        event EventHandler<AttackTargetEventArgs> AttackTargeting;
        event EventHandler<AttackTargetEventArgs> AttackTargeted;
        event EventHandler<SpellTargetEventArgs> SpellTargeting;
        event EventHandler<SpellTargetEventArgs> SpellTargeted;

        /// <summary>
        ///     Gates an attack behind whether the target can be targeted for attacks.
        /// </summary>
        void TargetAttack(Actor attacker, Action attackCallback, bool forceTargeting = false);

        /// <summary>
        ///     Gates a spell cast behind whether the target can be targeted for spells.
        ///     This differs from affect, which captures all spell effects.
        /// </summary>
        void TargetSpell(Action spellCallback, bool forceTargeting = false);

        /// <summary>
        ///     Gates a spell behind whether the target can be affected by spells.
        ///     This differs from targeting, which is specifically for being the target of a spell cast.
        /// </summary>
        void AffectSpell(Action spellCallback, bool forceTargeting = false);
    }
}