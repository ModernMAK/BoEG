using System;
using UnityEngine;

namespace Framework.Core.Modules
{
    public class Targetable : Module, ITargetable
    {
        public bool AllowAttackTargets { get; private set; }
        public bool AllowSpellTargets { get; private set; }

        public event TargetEventHandler AttackTargeting;
        public event TargetEventHandler AttackTargeted;
        public event SpellTargetEventHandler SpellTargeting;
        public event SpellTargetEventHandler SpellTargeted;

        public void TargetAttack(GameObject attacker, Action attackCallback, bool forceTargeting = false)
        {
            //If we don't force the target and we don't allow targeting, do nothing
            if (!forceTargeting && !AllowAttackTargets)
                return;

            OnAttackTargeting(attacker);
            attackCallback();
            OnAttackTargeted(attacker);
        }
        

        public void TargetSpell(SpellTargetEventArgs args, Action spellCallback, bool forceTargeting = false)
        {
            //If we don't force the target and we don't allow targeting, do nothing
            if (!forceTargeting && !AllowSpellTargets)
                return;

            OnSpellTargeting(args);
            spellCallback();
            OnSpellTargeted(args);
        }

        public void AffectSpell(Action spellCallback, bool forceTargeting = false)
        {
            if (forceTargeting || AllowSpellTargets)
                spellCallback();
        }

        private void OnAttackTargeting(GameObject attacker)
        {
            if (AttackTargeting != null)
            {
                AttackTargeting(attacker);
            }
        }

        private void OnAttackTargeted(GameObject attacker)
        {
            if (AttackTargeted != null)
            {
                AttackTargeted(attacker);
            }
        }

        private void OnSpellTargeting(SpellTargetEventArgs args)
        {
            if (SpellTargeting != null)
            {
                SpellTargeting(args);
            }
        }

        private void OnSpellTargeted(SpellTargetEventArgs args)
        {
            if (SpellTargeted != null)
            {
                SpellTargeted(args);
            }
        }
    }
}