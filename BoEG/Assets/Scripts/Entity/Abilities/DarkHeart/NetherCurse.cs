using System;
using Core;
using Modules.Abilityable;
using Modules.Healthable;
using Modules.Teamable;
using Triggers;
using UnityEngine;
using Util;

namespace Entity.Abilities.DarkHeart
{
    [CreateAssetMenu(fileName = "DarkHeart_NetherCurse.asset", menuName = "Ability/DarkHeart/Nether Curse")]
    public class NetherCurse : Ability
    {

        [SerializeField] private float[] _manaCost;
        [SerializeField] private float[] _castRange;
        [SerializeField] private TickData[] _tickInfo;
        [SerializeField] private float[] _tickDamage;
        [SerializeField] private float[] _areaOfEffect;


        public override float ManaCost
        {
            get { return GetLeveledData(_manaCost); }
        }

        public override float CastRange
        {
            get { return GetLeveledData(_castRange); }
        }

        private TickData Data
        {
            get { return GetLeveledData(_tickInfo); }
        }

        private float AreaOfEffect
        {
            get { return GetLeveledData(_areaOfEffect); }
        }

        private float TickDamage
        {
            get { return GetLeveledData(_tickDamage); }
        }


        [SerializeField] private GameObject _debugPrefab;
        private TickActionContainer<CurseInstance> _curseInstances;




        public override void Terminate()
        {
            //Terminate CurseInstances if they should go away after dying
//            _curseInstances.Terminate();
        }


        protected override void Initialize()
        {
            _curseInstances = new TickActionContainer<CurseInstance>();

            _spellRangeGameobject = Instantiate(_spellRangePrefab);
            _spellRangeGameobject.SetActive(false);
            _spellRangeVisualizer = _spellRangeGameobject.GetComponent<SpellRangeVisualizer>();
            
            _spellAoeGameobject = Instantiate(_spellAoePrefab);
            _spellAoeGameobject.SetActive(false);
            _spellAoeVisualizer = _spellAoeGameobject.GetComponent<SpellAoeVisualizer>();
        }

        [SerializeField] private GameObject _spellAoePrefab;
        [SerializeField] private GameObject _spellRangePrefab;
        private GameObject _spellAoeGameobject;
        private SpellAoeVisualizer _spellAoeVisualizer;
        private GameObject _spellRangeGameobject;
        private SpellRangeVisualizer _spellRangeVisualizer;

        protected override void Prepare()
        {        
        }

        protected override void CancelPrepare()
        {
            _spellAoeGameobject.SetActive(false);
            _spellRangeGameobject.SetActive(false);
        }

        protected override void Cast()
        {
            RaycastHit hit;
            if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
                return;

            if (!InCastRange(hit.point))
                return;
            
            SpendMana();
            GroundCast(hit.point);
        }

        public override void GroundCast(Vector3 point)
        {
            _curseInstances.Add(new CurseInstance(this, point));
        }

        public override void Step(float deltaTick)
        {
            if (Preparing && IsLeveled)
            {
                _spellRangeVisualizer.SetStart(Self.transform);
                _spellRangeVisualizer.SetRange(CastRange);
                _spellAoeVisualizer.SetAoeSize(AreaOfEffect);
                RaycastHit hit;
                if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
                {
//                    _spellAoeGameobject.SetActive(false);
                }
                else
                {
                    _spellAoeGameobject.SetActive(true);
                    _spellAoeVisualizer.SetPoint(hit.point);
                    _spellRangeGameobject.SetActive(true);
                    _spellRangeVisualizer.SetEnd(hit.point);
                }
            }
        }




        public override void PhysicsStep(float deltaTick)
        {
            _curseInstances.Tick(deltaTick);
        }



        private class CurseInstance : DotTickAction
        {
            public CurseInstance(NetherCurse curse, Vector3 position) :
                base(curse.Data)
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