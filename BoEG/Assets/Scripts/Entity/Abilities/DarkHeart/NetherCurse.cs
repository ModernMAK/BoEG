using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Modules.Abilityable;
using Modules.Abilityable.Ability;
using Modules.Healthable;
using Modules.Teamable;
using Old.Modules.Levelable;
using Triggers;
using UnityEngine;
using Util;

namespace Entity.Abilities.DarkHeart
{
    [CreateAssetMenu(fileName = "DarkHeart_NetherCurse.asset", menuName = "Ability/DarkHeart/Nether Curse")]
    public class NetherCurse : Ability
    {
        [Serializable]
        public struct NetherCurseData
        {
//            [SerializeField] private float[] _manaCost;
//            [SerializeField] private float[] _castRange;
            [SerializeField] private TickData[] _tickInfo;
            [SerializeField] private float[] _tickDamage;
            [SerializeField] private float[] _areaOfEffect;

            public int Length
            {
                //Just pick any, the property drawer ensures they should all be the same
                get { return _tickInfo.Length; }
            }


            public float[] AreaOfEffect
            {
                get { return _areaOfEffect; }
            }

//            public float[] ManaCost
//            {
//                get { return _manaCost; }
//            }
//
//            public float[] CastRange
//            {
//                get { return _castRange; }
//            }

            public TickData[] TickInfo
            {
                get { return _tickInfo; }
            }

            public float[] TickDamage
            {
                get { return _tickDamage; }
            }
        }

        [SerializeField] private NetherCurseData _data;

//        public override float ManaCost
//        {
//            get { return GetLeveledData(_data.ManaCost); }
//        }
//
//        public override float CastRange
//        {
//            get { return GetLeveledData(_data.CastRange); }
//        }

        private int TicksRequired
        {
            get { return LevelData.GetLeveledData(_data.TickInfo).TicksRequired; }
        }

        private float AreaOfEffect
        {
            get { return LevelData.GetLeveledData(_data.AreaOfEffect); }
        }

        private float TickDamage
        {
            get { return LevelData.GetLeveledData(_data.TickDamage); }
        }

        private float TickDuration
        {
            get { return LevelData.GetLeveledData(_data.TickInfo).Duration; }
        }

        [SerializeField] private GameObject _debugPrefab;
        private TickActionContainer<CurseInstance> _curseInstances;


//        protected override int MaxLevel
//        {
//            get { return _data.Length; }
//        }


        public override void Terminate()
        {
            //Terminate CurseInstances if they should go away after dying
//            _curseInstances.Terminate();
        }


        protected override void Initialize()
        {
            base.Initialize();
            _visualizer = Instantiate(_visualizer);
            _visualizer.Initialize();            
            _curseInstances = new TickActionContainer<CurseInstance>();


            var damage = new Damage(TickDamage, DamageType.Magical, Self);
            var sphereTrigger = new SphereTriggerMethod();
            var selector = new TriggerSelector(new Trigger(sphereTrigger));
            var affector = new TeamAffector.Enemy(Self.GetComponent<ITeamable>());
            var effect = new DamageEffect(damage);
            var helper = new Helper(selector,affector,effect);
        }

        
        [SerializeField] private SpellVisualizer _visualizer;

        public class SpellVisualizer : ScriptableObject
        {
            [SerializeField] private GameObject _spellAoePrefab;
            [SerializeField] private GameObject _spellRangePrefab;
            private GameObject _spellAoeGameobject;
            private SpellAoeVisualizer _spellAoeVisualizer;
            private GameObject _spellRangeGameobject;
            private SpellRangeVisualizer _spellRangeVisualizer;

            public void Initialize()
            {
                _spellRangeGameobject = Instantiate(_spellRangePrefab);
                _spellRangeGameobject.SetActive(false);
                _spellRangeVisualizer = _spellRangeGameobject.GetComponent<SpellRangeVisualizer>();

                _spellAoeGameobject = Instantiate(_spellAoePrefab);
                _spellAoeGameobject.SetActive(false);
                _spellAoeVisualizer = _spellAoeGameobject.GetComponent<SpellAoeVisualizer>();
            }


            public SpellVisualizer SetStart(Transform t)
            {
                _spellRangeVisualizer.SetStart(t);
                return this;
            }

            public SpellVisualizer SetRangeStart(Vector3 p)
            {
                _spellRangeVisualizer.SetStart(p);
                return this;
            }

            public SpellVisualizer SetRangeEnd(Transform t)
            {
                _spellRangeVisualizer.SetEnd(t);
                return this;
            }

            public SpellVisualizer SetRangeEnd(Vector3 p)
            {
                _spellRangeVisualizer.SetEnd(p);
                return this;
            }

            public SpellVisualizer SetRangeDistance(float r)
            {
                _spellRangeVisualizer.SetRange(r);
                return this;
            }

            public SpellVisualizer SetAoeSize(float r)
            {
                _spellAoeVisualizer.SetAoeSize(r);
                return this;
            }

            public SpellVisualizer SetAoePoint(Vector3 p)
            {
                _spellAoeVisualizer.SetPoint(p);
                return this;
            }

            public SpellVisualizer SetActive(bool active)
            {
                _spellAoeGameobject.SetActive(active);
                _spellRangeGameobject.SetActive(active);
                return this;
            }

//            public void Display()
//            {
//            }
        }
//
//        protected override void Prepare()
//        {        
//        }
//
//        protected void CancelPrepare()
//        {
//            _visualizer.SetActive(false);
//        }
//
//        protected override void Cast()
//        {
//            RaycastHit hit;
//            if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
//                return;
//
//            if (!InCastRange(hit.point))
//                return;
//            
//            SpendMana();
//            GroundCast(hit.point);
//        }
//
//        public override void GroundCast(Vector3 point)
//        {
//            _curseInstances.Add(new CurseInstance(this, point));
//        }
//
        public override void Step(float deltaTick)
        {
            if (true) //Preparing && IsLeveled)
            {
                
                RaycastHit hit;
                if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
                {
//                    _spellAoeGameobject.SetActive(false);
                }
                else
                {
                    _visualizer.SetActive(true).SetAoePoint(hit.point).SetRangeEnd(hit.point);
                }
            }
        }


        public override void PhysicsStep(float deltaTick)
        {
            _curseInstances.Tick(deltaTick);
        }


        [SerializeField] private AbilityManacost.Scalar _abilityManacost;

        protected override AbilityManacost ManacostData
        {
            get { return _abilityManacost; }
        }

        [SerializeField] private AbilityLevel _abilityLevel;

        protected override AbilityLevel LevelData
        {
            get { return _abilityLevel; }
        }

        [SerializeField] private AbilityCastRange.Scalar _abilityCastRange;

        protected override AbilityCastRange CastRangeData
        {
            get { return _abilityCastRange; }
        }

        [SerializeField] private AbilityCooldown.Scalar _abilityCooldown;

        protected override AbilityCooldown CooldownData
        {
            get { return _abilityCooldown; }
        }

        private class Helper
        {
            public Helper(Selector selector, Affector affector, Effect effect)
            {
                _selector = selector;
                _affector = affector;
                _effect = effect;                
            }

            public Helper SetSelector(Selector selector)
            {
                _selector = selector;
                return this;
            }
            public Helper SetAffector(Affector affector)
            {
                _affector = affector;
                return this;
            }
            public Helper SetEffect(Effect effect)
            {
                _effect = effect;
                return this;
            }
            private Selector _selector;
            private Affector _affector;
            private Effect _effect;
            
            public void Run()
            {
                Action<Selector, Affector, Effect> f = EffectAffectSelectUtil.Apply;
                f.Apply(_selector).Apply(_affector).Apply(_effect).Invoke();
            }
        }

        private class CurseInstance : DotTickAction
        {
            public CurseInstance(NetherCurse curse, Vector3 position) :
                base(curse.TicksRequired, curse.TickDuration)
            {
                var triggerAura = new SphereTriggerMethod();
                triggerAura.SetRadius(curse.AreaOfEffect).SetPosition(position)
                    .SetLayerMask((int) LayerMaskHelper.Entity);
                _trigger = new Trigger(triggerAura);

                _trigger.Enter += OnUnitEnter;
                _trigger.Stay += OnUnitEnter;

                _self = curse.Self;
                _damageOverTime = curse.TickDamage;
                _teamable = _self.GetComponent<ITeamable>();


                _debugInst = Instantiate(curse._debugPrefab);
                _debugInst.transform.position = position;
                _debugInst.transform.localScale = Vector3.one * curse.AreaOfEffect;
            }

            private readonly GameObject _self;

            private readonly float _damageOverTime;
            private readonly Trigger _trigger;

            private readonly ITeamable _teamable;
            private readonly GameObject _debugInst;

            public override void Terminate()
            {
                Destroy(_debugInst);
            }

            protected override void Logic()
            {
                _trigger.PhysicsStep(); //Each tick, run a physics step to check if anyone is inside the area of effect
                //The Trigger's Physics tick will 
            }

            private void OnUnitEnter(GameObject go)
            {
                var healthable = go.GetComponent<IHealthable>();
                var teamable = go.GetComponent<ITeamable>();
                if (DoneTicking)
                    return;
                if (_self == go)
                    return;
                if (healthable == null)
                    return;
                if (teamable != null && _teamable != null && _teamable.Team == teamable.Team &&
                    _teamable.Team != null)
                    return;
                var damage = new Damage(_damageOverTime, DamageType.Magical, _self);
                ApplyDamageOverTime(healthable, damage);
            }
        }
    }
}