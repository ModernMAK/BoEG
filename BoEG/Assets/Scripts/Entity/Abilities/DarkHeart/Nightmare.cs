using Core;
using Modules.Healthable;
using Modules.Abilityable;
using UnityEngine;
using System.Collections.Generic;
using Modules.Magicable;
using Util;


namespace Entity.Abilities.DarkHeart
{
    [CreateAssetMenu(fileName = "DarkHeart_Nightmare.asset", menuName = "Ability/DarkHeart/Nightmare")]
    public class Nightmare : Ability
    {
        [SerializeField] private float _duration;
        [SerializeField] private int _ticks;
        [SerializeField] private float _tickDamage;
        private GameObject _self;
        [SerializeField] private GameObject _nightmareFX;
        [SerializeField] private float _manaCost;
        [SerializeField] private float _castRange;

        private IMagicable _magicable;

        //Nightmare Duration
        //Nightmare AttackDamage On Completion


        //Nightmare AttackDamage Threshold


        public override void Terminate()
        {
            for (int i = 0; i < _nightmareInstances.Count; i++)
            {
                var inst = _nightmareInstances[i];
                inst.Terminate();
            }
            _nightmareInstances.Clear();
        }


        public override void Initialize(GameObject go)
        {
            _self = go;
            _nightmareInstances = new List<NightmareInstance>();
            _magicable = go.GetComponent<IMagicable>();
//            throw new System.NotImplementedException();
        }

        public override void Trigger()
        {
            RaycastHit hit;
            if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)) return;

            if ((hit.point - _self.transform.position).sqrMagnitude > _castRange * _castRange)
                return;


            if (_magicable.ManaPoints < _manaCost)
                return;

            var col = hit.collider;
            var go = col.gameObject;
            if (col.attachedRigidbody != null)
                go = col.attachedRigidbody.gameObject;

            _magicable.ModifyMana(-_manaCost, _self);
            ApplyNightmare(go);
        }

        public override void PhysicsTick(float deltaTick)
        {
            for (int i = 0; i < _nightmareInstances.Count; i++)
            {
                var inst = _nightmareInstances[i];
                inst.PhysicsTick(deltaTick);

                if (inst.IsDone)
                {
                    inst.Terminate();
                    _nightmareInstances.RemoveAt(i);
                    i--;
                }
            }
        }

        private List<NightmareInstance> _nightmareInstances;

        public void ApplyNightmare(GameObject go)
        {
            _nightmareInstances.Add(new NightmareInstance(this, _self, go));
        }


        private class NightmareInstance
        {
            private TickHelper _helper;
            private float _damage;
            private GameObject _target;
            private IHealthable _healthable;
            private GameObject _self;
            private GameObject _fx;

            public NightmareInstance(Nightmare nightmare, GameObject self, GameObject target)
            {
                _self = self;
                _helper = TickHelper.CreateFromDuration(nightmare._duration, nightmare._ticks);
                _damage = nightmare._tickDamage;
                _target = target;
                _healthable = target.GetComponent<IHealthable>();
                _fx = Instantiate(nightmare._nightmareFX, _target.transform);
            }

            public void PhysicsTick(float deltaTick)
            {
                _helper.AdvanceTime(deltaTick);
                Logic();
                _helper.AdvanceTicks();
            }

            private void Logic()
            {
                if(_healthable.HealthPercentage.SafeEquals(0f))
                    return;
                var ticks = _helper.TicksToPerform;
                for (var i = 0; i < ticks; i++)
                {
                    _healthable.TakeDamage(new Damage(_damage, DamageType.Magical, _self));
                }
            }

            public bool IsDone
            {
                get { return _helper.DoneTicking; }
            }

            public void Terminate()
            {
                Destroy(_fx);
            }
        }
    }

//    public class NightmareEffect : Effect
//    {
//        public NightmareEffect(float duration, float completionDamage)
//        {
//            _timeDuration = duration;
//            _completionDamage = completionDamage;
//        }
//        private float _timeStarted, _timeDuration;
//        private float _completionDamage;
//        private IHealthable _targetHealthable;
//        public override void Initialize(GameObject target)
//        {
//            base.Initialize(target);
//            _timeStarted = Time.time;
//            _targetHealthable = Target.GetComponent<IHealthable>();
//        }
//
//        public override void PreTick()
//        {
//            _timeStarted
//        }
//
//        public override void PostTick()
//        {
//            if (_timeStarted + _timeDuration <= Time.time)
//            {
//                
//                Terminate();   
//            }
//        }
//
//        private void ApplyDamage()
//        {
//            _targetHealthable.TakeDamage(new Damage());
//        }
//    }
}