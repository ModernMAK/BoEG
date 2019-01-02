using System;
using UnityEngine;

namespace Framework.Core.Modules
{
    public interface ITargetable
    {
        bool AllowAttackTargets { get; }
        bool AllowSpellTargets { get; }
        event TargetEventHandler AttackTargeting;
        event TargetEventHandler AttackTargeted;
        event SpellTargetEventHandler SpellTargeting;
        event SpellTargetEventHandler SpellTargeted;
        void TargetAttack(GameObject attacker, Action attackCallback, bool forceTargeting = false);
        void TargetSpell(SpellTargetEventArgs args, Action spellCallback, bool forceTargeting = false);
        void AffectSpell(Action spellCallback, bool forceTargeting = false);
    }
}