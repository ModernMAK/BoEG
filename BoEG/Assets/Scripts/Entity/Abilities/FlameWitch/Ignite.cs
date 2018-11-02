using Core;
using Modules.Abilityable;
using Modules.Abilityable.Ability;
using Modules.Healthable;
using Triggers;
using UnityEngine;
using Util;

namespace Entity.Abilities.FlameWitch
{
    [CreateAssetMenu(fileName = "FlameWitch_Ignite.asset", menuName = "Ability/FlameWitch/Wildfire")]
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
            _aeraOfEffectDamageOverTimeTicker = new WildfireTickAction(_tickInfo.TicksRequired, _tickInfo.Duration, _trigger, damage);
        }
        
        
        private class WildfireTickAction : DotTickAction
        {
            public WildfireTickAction(int ticksRequired, float tickDuration, Trigger trigger, Damage damage) : base(ticksRequired, tickDuration)
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

    [CreateAssetMenu(fileName = "FlameWitch_Ignite.asset", menuName = "Ability/FlameWitch/Ignite")]
    public class Ignite : Ability
    {
        [SerializeField] private float _manaCost = 100f;
        [SerializeField] private float _damage = 100f;
        [SerializeField] private float _castRange = 5f;



        public override void Terminate()
        {
            //I'll Be Back
            //To add stuff
        }

//        protected override void Cast()
//        {
//            RaycastHit hit;
//            if (!RaycastCamera(out hit, LayerMaskHelper.Entity))
//                return;
//
//            if (!InCastRange(hit.point))
//                return;
//
//            SpendMana();
//            UnitCast(GetEntity(hit));
//        }

//        protected override void Prepare()
//        {
//        }
//
//        public override void UnitCast(GameObject target)
//        {
//            var damage = new Damage(_damage, DamageType.Magical, Self);
//            var healthable = target.GetComponent<IHealthable>();
//            healthable.TakeDamage(damage);
//        }

        public static bool RaycastCamera(out RaycastHit hitinfo, LayerMaskHelper layerMask = (LayerMaskHelper) 0)
        {
            var lm = (int) layerMask;
            if (lm == 0)
                lm = (int) (LayerMaskHelper.Entity | LayerMaskHelper.Trigger | LayerMaskHelper.World);
            return Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitinfo, lm);
        }

        public static GameObject GetEntity(RaycastHit hit)
        {
            var go = hit.collider.gameObject;
            var rb = hit.collider.GetComponent<Rigidbody>();

            if (rb != null)
                go = rb.gameObject;

            var entity = go.GetComponent<Entity>();


            if (entity == null)
                entity = go.GetComponentInChildren<Entity>();

            if (entity == null)
                entity = go.GetComponentInParent<Entity>();

            if (entity != null)
                return entity.gameObject;
            else return null;
        }
    }
}