//using System;
//using Core;
//using Framework.Types;
//using Framework.Utility;
//using Modules.Healthable;
//using Modules.Abilityable;
//using UnityEngine;
//using Util;
//
//namespace Entity.Abilities.DarkHeart
//{
//    [CreateAssetMenu(fileName = "DarkHeart_Nightmare.asset", menuName = "Ability/DarkHeart/Nightmare")]
//    public class Nightmare : Ability
//    {
//        [Serializable]
//        public struct NightmareData
//        {
//            [SerializeField] private float[] _manaCost;
//            [SerializeField] private float[] _castRange;
//            [SerializeField] private TickData[] _tickInfo;
//            [SerializeField] private float[] _totalDamage;
//
//            public int Length
//            {
//                //Just pick any, the property drawer ensures they should all be the same
//                get { return _manaCost.Length; }
//            }
//
//            public float[] ManaCost
//            {
//                get { return _manaCost; }
//            }
//
//            public float[] CastRange
//            {
//                get { return _castRange; }
//            }
//
//            public TickData[] TickInfo
//            {
//                get { return _tickInfo; }
//            }
//
//            public float[] TotalDamage
//            {
//                get { return _totalDamage; }
//            }
//        }
//
//        [SerializeField] private NightmareData _data;
//
//        public override float CastRange
//        {
//            get { return GetLeveledData(_data.CastRange); }
//        }
//
//        public override float ManaCost
//        {
//            get { return GetLeveledData(_data.ManaCost); }
//        }
//
//        private TickData TickInfo
//        {
//            get { return GetLeveledData(_data.TickInfo); }
//        }
//
//        private float TotalDamage
//        {
//            get { return GetLeveledData(_data.TotalDamage); }
//        }
//
//        protected override int MaxLevel
//        {
//            get { return _data.Length; }
//        }
//
//        [SerializeField] private GameObject _nightmareFX;
//
//        [SerializeField] private GameObject _spellRangePrefab;
//        private GameObject _spellRangeGameobject;
//
//        private SpellRangeVisualizer _spellRangeVisualizer;
//        //Nightmare AttackDamage On Completion
//
//
//        //Nightmare AttackDamage Threshold
//
//
//        public override void Terminate()
//        {
//            _nightmareInstances.Terminate();
//        }
//
//
//        protected override void Initialize()
//        {
//            _nightmareInstances = new TickActionContainer<NightmareInstance>();
//            _spellRangeGameobject = Instantiate(_spellRangePrefab);
//            _spellRangeGameobject.SetActive(false);
//            _spellRangeVisualizer = _spellRangeGameobject.GetComponent<SpellRangeVisualizer>();
////            throw new System.NotImplementedException();
//        }
//
//        protected override void CancelPrepare()
//        {
//            _spellRangeGameobject.SetActive(false);
//        }
//
//        public override void Step(float deltaTick)
//        {
//            if (Preparing && IsLeveled)
//            {
//                _spellRangeVisualizer.SetStart(Self.transform);
//                _spellRangeVisualizer.SetRange(CastRange);
//                RaycastHit hit;
//                if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
//                {
////                    _spellAoeGameobject.SetActive(false);
//                }
//                else
//                {
//                    _spellRangeGameobject.SetActive(true);
//                    _spellRangeVisualizer.SetEnd(hit.point);
//                }
//            }
//        }
//
//        protected override void Cast()
//        {
//            RaycastHit hit;
//            if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)) return;
//
//            if (!InCastRange(hit.point))
//                return;
//
//            var col = hit.collider;
//            var go = col.gameObject;
//            if (col.attachedRigidbody != null)
//                go = col.attachedRigidbody.gameObject;
//
//            SpendMana();
//            UnitCast(go);
//        }
//
//        public override void PhysicsStep(float deltaTick)
//        {
//            _nightmareInstances.Tick(deltaTick);
//        }
//
//        private TickActionContainer<NightmareInstance> _nightmareInstances;
//
//        public override void UnitCast(GameObject target)
//        {
//            var iHealthable = target.GetComponent<IHealthable>();
//            if (iHealthable != null)
//            {
//                _nightmareInstances.Add(new NightmareInstance(this, Self, target));
//            }
//        }
//
//        private class NightmareInstance : DotTickAction
//        {
//            private readonly float _damage;
//            private readonly GameObject _target;
//            private readonly IHealthable _healthable;
//            private readonly GameObject _self;
//            private readonly GameObject _fx;
//
//            public NightmareInstance(Nightmare nightmare, GameObject self, GameObject target) :
//                base(nightmare.TickInfo)
//            {
//                _self = self;
//                _damage = nightmare.TotalDamage / nightmare.TickInfo.Duration;
//                _target = target;
//                _healthable = target.GetComponent<IHealthable>();
//                _fx = Instantiate(nightmare._nightmareFX, _target.transform);
//            }
//
//
//            protected override void Logic()
//            {
//                if (_healthable.Normal.SafeEquals(0f))
//                    return;
//                ApplyDamageOverTime(_healthable, new Damage(_damage, DamageType.Magical, _self));
//            }
//
//            public override void Terminate()
//            {
//                Destroy(_fx);
//            }
//        }
//    }
//
////    public class NightmareEffect : Effect
////    {
////        public NightmareEffect(float duration, float completionDamage)
////        {
////            _timeDuration = duration;
////            _completionDamage = completionDamage;
////        }
////        private float _timeStarted, _timeDuration;
////        private float _completionDamage;
////        private IHealthable _targetHealthable;
////        public override void Initialize(GameObject target)
////        {
////            base.Initialize(target);
////            _timeStarted = Time.time;
////            _targetHealthable = Target.GetComponent<IHealthable>();
////        }
////
////        public override void PreStep()
////        {
////            _timeStarted
////        }
////
////        public override void PostStep()
////        {
////            if (_timeStarted + _timeDuration <= Time.time)
////            {
////                
////                Terminate();   
////            }
////        }
////
////        private void ApplyDamage()
////        {
////            _targetHealthable.TakeDamage(new ApplyDamageOverTime());
////        }
////    }
//}