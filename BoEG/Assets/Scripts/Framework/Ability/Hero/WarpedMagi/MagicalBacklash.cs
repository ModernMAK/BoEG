using System;
using Framework.Core.Modules;
using Framework.Types;
using UnityEngine;
using UnityEngine.Analytics;

namespace Framework.Ability.Hero.WarpedMagi
{
    /// <summary>
    /// [x] Ignores Spell Targetability
    /// </summary>
    public class MagicalBacklash : BetterAbility
    {
        private ITargetable _targetable;

        protected override void Instantiate()
        {
            base.Instantiate();
            _targetable = GetComponent<ITargetable>();
        }

        protected override void Spawn()
        {
            base.Spawn();
//            _targetable.SpellTargeted += OnTargetedForSpell;
        }

        protected override void Despawn()
        {
            base.Despawn();
//            _targetable.SpellTargeted -= OnTargetedForSpell;
        }

        private Damage CreateDamage(float manaSpent)
        {
            const float ManaAsDamage = 0.75f;
            var damage = manaSpent * ManaAsDamage;
            return new Damage(damage, DamageType.Magical, DamageModifiers.Ability);
        }

        private void OnTargetedForSpell(SpellTargetEventArgs args)
        {
            var go = args.Caster;
            var targetable = go.GetComponent<ITargetable>();
            var damagable = go.GetComponent<IDamageTarget>();
            const bool PierceSpellImmunity = true;

            if (damagable != null && targetable != null)
            {
                var damage = CreateDamage(args.ManaSpent);
                Action callback = (() => damagable.TakeDamage(damage));
                //No longer uses SpellArgs since we use AffectSpell instead of TargetSpells
                //var spellArgs = new SpellTargetEventArgs(gameObject, 0f, damage);
                //We still use AffectSpell even though we Pierce Spell Immunity in case it no longer pierces or affect gets more logic
                targetable.AffectSpell(callback, PierceSpellImmunity);
            }
        }
    }
}