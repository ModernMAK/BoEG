using System;
using Framework.Types;
using UnityEngine;

namespace Framework.Core.Modules
{
    public class NewTargetable : ITargetable
    {
        public bool AllowAttackAffects { get; private set; }
        public bool AllowSpellAffects { get; private set; }


        public bool AllowAttackTargets { get; private set; }

        public bool AllowSpellTargets { get; private set; }

        public event EventHandler<AttackTargetEventArgs> AttackTargeting;
        public event EventHandler<AttackTargetEventArgs> AttackTargeted;
        public event EventHandler<SpellTargetEventArgs> SpellTargeting;
        public event EventHandler<SpellTargetEventArgs> SpellTargeted;

        public void TargetAttack(Actor attacker, Action attackCallback, bool forceTargeting = false)
        {
            var args = new AttackTargetEventArgs(attacker, new Damage(0f, DamageType.Physical));
            OnAttackTargeting(args);
            attackCallback();
            OnAttackTargeted(args);
        }

        public void TargetSpell(Action spellCallback, bool forceTargeting = false)
        {
            var args = new SpellTargetEventArgs(null, 0f, new Damage(0f, DamageType.Physical));
            OnSpellTargeting(args);
            AffectSpell(spellCallback, forceTargeting);
            OnSpellTargeted(args);
        }

        public void AffectSpell(Action spellCallback, bool forceTargeting = false)
        {
            spellCallback();
        }

        protected virtual void OnAttackTargeting(AttackTargetEventArgs e)
        {
            AttackTargeting?.Invoke(this, e);
        }

        protected virtual void OnAttackTargeted(AttackTargetEventArgs e)
        {
            AttackTargeted?.Invoke(this, e);
        }

        protected virtual void OnSpellTargeting(SpellTargetEventArgs e)
        {
            SpellTargeting?.Invoke(this, e);
        }

        protected virtual void OnSpellTargeted(SpellTargetEventArgs e)
        {
            SpellTargeted?.Invoke(this, e);
        }
    }

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
            AttackTargeting?.Invoke(this, args);
        }

        private void OnAttackTargeted(AttackTargetEventArgs args)
        {
            AttackTargeted?.Invoke(this, args);
        }

        private void OnSpellTargeting(SpellTargetEventArgs args)
        {
            SpellTargeting?.Invoke(this, args);
        }

        private void OnSpellTargeted(SpellTargetEventArgs args)
        {
            SpellTargeted?.Invoke(this, args);
        }
    }
}