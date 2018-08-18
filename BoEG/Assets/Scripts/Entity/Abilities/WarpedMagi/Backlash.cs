using Core;
using Modules.Abilityable;
using Modules.Magicable;
using Triggers;
using Modules.Healthable;
using UnityEngine;
using Util;

namespace Entity.Abilities.WarpedMagi
{
    [CreateAssetMenu(fileName = "WarpedMagi_Backlash.asset", menuName = "Ability/WarpedMagi/Backlash")]
    public class Backlash : Ability
    {
        //Necromancy Chance
        [SerializeField] [Range(0f, 1f)] private float _backlashPercentage = 0.1f;
        [SerializeField] private float _backlashRadius = 1f;
        private GameObject _self;
        private Trigger _trigger;
        private SphereTriggerMethod _triggerAura;


        public override void Initialize(GameObject go)
        {
            _self = go;
            _triggerAura = new SphereTriggerMethod();
            _triggerAura.SetRadius(_backlashRadius).SetFollow(_self).SetLayerMask((int) LayerMaskHelper.Entity);
            _trigger = new Trigger(_triggerAura);
            _trigger.Enter += Enter;
            _trigger.Exit += Exit;
        }

        public override void PhysicsTick(float deltaTime)
        {
            _trigger.PhysicsTick();
        }
        public override void Terminate()
        {
            foreach (var go in _trigger.Colliders)
            {
                Exit(go);
            }
        }

        private void Enter(GameObject go)
        {
            if(go == _self)
                return;
            
            var magicable = go.GetComponent<IMagicable>();
            if (magicable != null)
                magicable.ManaModified += ManaModifiedCallback;
        }

        private void Exit(GameObject go)
        {
            var magicable = go.GetComponent<IMagicable>();
            if (magicable != null)
                magicable.ManaModified -= ManaModifiedCallback;
        }

        private void ManaModifiedCallback(ManaModifiedEventArgs args)
        {
            var amountSpent = Mathf.Max(0f, -args.Modified);
            var source = args.Source;
            var target = args.Owner;
            IHealthable healthable = (target != null) ? target.GetComponent<IHealthable>() : null;

            var backlashAmount = amountSpent * _backlashPercentage;
            if (healthable != null && source == target && backlashAmount > 0)
            {
                var damage = new Damage(backlashAmount, DamageType.Magical, _self);
                healthable.TakeDamage(damage);
            }
        }

        public override void Trigger()
        {
        }
    }
}