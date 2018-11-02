using Core;
using Modules.Abilityable;
using Modules.Healthable;
using Triggers;
using UnityEngine;
using Util;

namespace Entity.Abilities.FlameWitch
{
    [CreateAssetMenu(fileName = "FlameWitch_Wildfire.asset", menuName = "Ability/FlameWitch/Wildfire")]
    public class Wildfire : Ability
    {
        [SerializeField] private float _areaOfEffect;
        [SerializeField] private TickData _tickInfo;
        [SerializeField] private float _damage;

        private WildfireTickAction _aeraOfEffectDamageOverTimeTicker;
        private Trigger _trigger;
        private SphereTriggerMethod _triggerMethod;


        protected override void Initialize()
        {
            _trigger = new Trigger(_triggerMethod);
            _triggerMethod = new SphereTriggerMethod();
            _triggerMethod.SetRadius(_areaOfEffect).SetFollow(Self).SetLayerMask((int) LayerMaskHelper.Entity);
            var damage = new Damage(_damage, DamageType.Pure, Self);
            _aeraOfEffectDamageOverTimeTicker = new WildfireTickAction(_tickInfo, _trigger, damage);
        }
        
        
        private class WildfireTickAction : DotTickAction
        {
            public WildfireTickAction(TickData data, Trigger trigger, Damage damage) : base(data)
            {
                _trigger = trigger;
                _damage = damage;
            }

            public void SetDamage(Damage damage)
            {
                _damage = damage;
            }
            
            private Damage _damage;
            private readonly Trigger _trigger;

            protected override void Logic()
            {
                _trigger.PhysicsStep();
                foreach (var col in _trigger.Colliders)
                {
                    var healthable = col.GetComponent<IHealthable>();
                        
                    ApplyDamageOverTime(healthable,_damage);
                }
            }
        }
        
        public override void Terminate()
        {
//            throw new System.NotImplementedException();            
        }

        public override void Step(float deltaTick)
        {
            _aeraOfEffectDamageOverTimeTicker.Tick(deltaTick);
        }
    }
}