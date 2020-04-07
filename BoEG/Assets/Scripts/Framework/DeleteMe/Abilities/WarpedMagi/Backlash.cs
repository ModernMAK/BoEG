using System;
using Framework.Ability;
using Framework.Core.Modules;
using Framework.Types;
using Triggers;
using UnityEngine;
using Util;

namespace Entity.Abilities.WarpedMagi
{
//    [CreateAssetMenu(fileName = "WarpedMagi_Backlash.asset", menuName = "Ability/WarpedMagi/Backlash")]
    public class Backlash : Ability
    {
        //Necromancy Chance
        [SerializeField] [Range(0f, 1f)] private float _backlashPercentage = 0.1f;
        [SerializeField] private float _backlashRadius = 1f;
        private Trigger _trigger;
        private SphereTriggerMethod _triggerAura;
        private GameObject Self;

        protected void Initialize()
        {
            _triggerAura = new SphereTriggerMethod();
            _triggerAura.SetRadius(_backlashRadius).SetFollow(Self).SetLayerMask((int) LayerMaskHelper.Entity);
            _trigger = new Trigger(_triggerAura);
            _trigger.Enter += Enter;
            _trigger.Exit += Exit;
        }

        public void PhysicsStep(float deltaTime)
        {
            _trigger.PhysicsStep();
        }

        public void Terminate()
        {
            foreach (var go in _trigger.Colliders)
            {
                Exit(this, go);
            }
        }

        private void Enter(object sender, GameObject go)
        {
            if (go == Self)
                return;

            var magicable = go.GetComponent<IMagicable>();
            if (magicable != null)
                magicable.Modified += ManaModifiedCallback;
        }

        private void Exit(object sender, GameObject go)
        {
            var magicable = go.GetComponent<IMagicable>();
            if (magicable != null)
                magicable.Modified -= ManaModifiedCallback;
        }

        private void ManaModifiedCallback(object sender, MagicableEventArgs args)
        {
//            var source = args.Source;
//            var target = args.Owner;
            GameObject source = null;
            GameObject target = null;

            var damageTarget = target?.GetComponent<IDamageTarget>();
            var targetable = target?.GetComponent<ITargetable>();

            Action callback = () =>
            {
                var amountSpent = Mathf.Max(0f, -args.Change);
                var backlashAmount = amountSpent * _backlashPercentage;
                var damage = new Damage(backlashAmount, DamageType.Magical, DamageModifiers.Ability);
                damageTarget?.TakeDamage(damage);
            };
            targetable?.AffectSpell(callback);
        }
    }
}