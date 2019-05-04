using System;
using UnityEngine;

namespace Framework.Core.Modules
{
    public interface ITargetable
    {
        bool AllowAttackTargets { get; }
        bool AllowSpellTargets { get; }
        event EventHandler<AttackTargetEventArgs> AttackTargeting;
        event EventHandler<AttackTargetEventArgs> AttackTargeted;
        event EventHandler<SpellTargetEventArgs> SpellTargeting;
        event EventHandler<SpellTargetEventArgs> SpellTargeted;
        void TargetAttack(Actor attacker, Action attackCallback, bool forceTargeting = false);
        void TargetSpell(Action spellCallback, bool forceTargeting = false);
        void AffectSpell(Action spellCallback, bool forceTargeting = false);
    }

}