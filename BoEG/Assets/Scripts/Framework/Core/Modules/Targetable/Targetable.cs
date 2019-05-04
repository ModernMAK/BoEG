using System;
using UnityEngine;

namespace Framework.Core.Modules
{
    public class Targetable : MonoBehaviour, ITargetable
    {
        public bool AllowAttackTargets { get; private set; }
        public bool AllowSpellTargets { get; private set; }
        public event EventHandler<AttackTargetEventArgs> AttackTargeting;
        public event EventHandler<AttackTargetEventArgs> AttackTargeted;
        public event EventHandler<SpellTargetEventArgs> SpellTargeting;
        public event EventHandler<SpellTargetEventArgs> SpellTargeted;


        public void TargetAttack(Actor attacker, Action attackCallback, bool forceTargeting = false)
        {
            //If we don't force the target and we don't allow targeting, do nothing
            if (!forceTargeting && !AllowAttackTargets)
                return;

            throw new NotImplementedException();
//            OnAttackTargeting(attacker);
//            attackCallback();
//            OnAttackTargeted(attacker);
        }
        


        public void TargetSpell(Action spellCallback, bool forceTargeting = false)
        {
            //If we don't force the target and we don't allow targeting, do nothing
            if (!forceTargeting && !AllowSpellTargets)
                return;
            throw new NotImplementedException();
//
//            var args = new SpellTargetEventArgs(caster);
//            OnSpellTargeting(args);
//            spellCallback();
//            OnSpellTargeted(args);
        }

        public void AffectSpell(Action spellCallback, bool forceTargeting = false)
        {
            if (forceTargeting || AllowSpellTargets)
                spellCallback();
        }

        private void OnAttackTargeting(AttackTargetEventArgs args)
        {
            AttackTargeting?.Invoke(this,args);
        }

        private void OnAttackTargeted(AttackTargetEventArgs args)
        {
            AttackTargeted?.Invoke(this,args);
        }

        private void OnSpellTargeting(SpellTargetEventArgs args)
        {
            SpellTargeting?.Invoke(this,args);
        }

        private void OnSpellTargeted(SpellTargetEventArgs args)
        {
            SpellTargeted?.Invoke(this,args);
        }
    }
}