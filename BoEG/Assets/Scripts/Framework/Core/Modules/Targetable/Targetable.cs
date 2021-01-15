using System;

namespace MobaGame.Framework.Core.Modules
{
    public class Targetable : ActorModule, ITargetable
    {
        public Targetable(Actor actor) : base(actor)
        {
            AllowAttackAffects = true;
            AllowSpellAffects = true;

            AllowAttackTargets = true;
            AllowSpellTargets = true;
        }


        public bool AllowAttackAffects { get; private set; }
        public bool AllowSpellAffects { get; private set; }
        public event EventHandler AttackTargetingChanged;
        public event EventHandler SpellTargetingChanged;


        public bool AllowAttackTargets { get; private set; }

        public bool AllowSpellTargets { get; private set; }

        public event EventHandler<AttackTargetEventArgs> AttackTargeting;
        public event EventHandler<AttackTargetEventArgs> AttackTargeted;
        public event EventHandler<SpellTargetEventArgs> SpellTargeting;
        public event EventHandler<SpellTargetEventArgs> SpellTargeted;

        public void TargetAttack(Action attackCallback, AttackTargetEventArgs args, bool forceTargeting = false)
        {
            if (forceTargeting || AllowAttackTargets)
            {
                OnAttackTargeting(args);
                AffectAttack(attackCallback, forceTargeting);
                OnAttackTargeted(args);
            }
        }

        public void AffectAttack(Action attackCallback, bool forceTargeting = false)
        {
            if (forceTargeting || AllowAttackAffects)
                attackCallback();
        }

        public void TargetSpell(Action spellCallback, SpellTargetEventArgs args, bool forceTargeting = false)
        {
            if (forceTargeting || AllowSpellTargets)
            {
                OnSpellTargeting(args);
                AffectSpell(spellCallback, forceTargeting);
                OnSpellTargeted(args);
            }
        }

        public void AffectSpell(Action spellCallback, bool forceTargeting = false)
        {
            if (forceTargeting || AllowSpellAffects)
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

}