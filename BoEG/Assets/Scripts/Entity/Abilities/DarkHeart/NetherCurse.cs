using System.Collections.Generic;
using Core;
using Modules.Abilityable;
using Modules.Healthable;
using Modules.Magicable;
using Modules.Teamable;
using Old;
using Triggers;
using UnityEngine;

namespace Entity.Abilities.WarpedMagi
{
    public class TickHelper
    {
        public TickHelper(int required, float interval)
        {
            TicksRequired = required;
            TickInterval = interval;
            TicksPerformed = 0;
            ElapsedTime = 0f;
        }

        public static TickHelper CreateFromDuration(float duration, int required)
        {
            var interval = duration / required;
            return new TickHelper(required, interval);
        }

        public static TickHelper CreateFromDuration(float duration, float interval)
        {
            var required = Mathf.CeilToInt(duration / interval);
            return new TickHelper(required, interval);
        }


        private int TicksRequired { get; set; }
        private int TicksPerformed { get; set; }
        private float TickInterval { get; set; }
        private float ElapsedTime { get; set; }

        public void AdvanceTime(float time)
        {
            ElapsedTime += time;
        }

        public int TicksToPerform
        {
            get
            {
                var ticksLeft = TicksRequired - TicksPerformed;
                var allowedTicks = Mathf.FloorToInt(ElapsedTime / TickInterval);
                return Mathf.Min(ticksLeft, allowedTicks);
            }
        }

        public bool DoneTicking
        {
            get { return TicksRequired <= TicksPerformed; }
        }

        public void AdvanceTicks()
        {
            var temp = TicksToPerform;
            TicksPerformed += temp;
            ElapsedTime -= TickInterval * temp;
        }
    }

    [CreateAssetMenu(fileName = "DarkHeart_NetherCurse.asset", menuName = "Ability/DarkHeart/Nether Curse")]
    public class NetherCurse : Ability
    {
        [SerializeField] private float _manaCost;
        [SerializeField] private float _castRange;
        [SerializeField] private float _netherCurseTickInterval;
        [SerializeField] private float _netherCurseAreaOfEffect;
        [SerializeField] private float _netherCurseDamagePerTick;
        [SerializeField] private float _netherCurseDuration;
        [SerializeField] private GameObject _debugPrefab;
        private GameObject _self;
        private IMagicable _magicable;
        private List<CurseInstance> _curseInstances;


        public override void Terminate()
        {
        }

        private class CurseInstance
        {
            public CurseInstance(NetherCurse curse, Vector3 position)
            {
                var triggerAura = new SphereTriggerMethod();
                triggerAura.SetRadius(curse._netherCurseAreaOfEffect).SetPosition(position)
                    .SetLayerMask((int) LayerMaskHelper.Entity);
                _trigger = new Trigger(triggerAura);

                _trigger.Enter += Damage;
                _trigger.Stay += Damage;

                _self = curse._self;
                _damageOverTime = curse._netherCurseDamagePerTick;
                _teamable = _self.GetComponent<ITeamable>();

                _tickHelper = TickHelper.CreateFromDuration(curse._netherCurseDuration, curse._netherCurseTickInterval);

                _debugInst = Instantiate(curse._debugPrefab);
                _debugInst.transform.position = position;
                _debugInst.transform.localScale = Vector3.one * curse._netherCurseAreaOfEffect;
            }

            private readonly GameObject _self;

            private TickHelper _tickHelper;
            private readonly float _damageOverTime;
            private readonly Trigger _trigger;

            private readonly ITeamable _teamable;
            private GameObject _debugInst;

            public void TerminateDebugInstance()
            {
                Destroy(_debugInst);
            }

            private void Damage(GameObject go)
            {
                var healthable = go.GetComponent<IHealthable>();
                var teamable = go.GetComponent<ITeamable>();
                var performingTicks = _tickHelper.TicksToPerform;
                for (int i = 0; i < performingTicks; i++)
                {
                    if (IsDone)
                        return;
                    if (_self == go)
                        return;
                    if (healthable == null)
                        return;
                    if (teamable != null && _teamable != null && _teamable.Team == teamable.Team &&
                        _teamable.Team != null)
                        return;
                    var damage = new Damage(_damageOverTime, DamageType.Magical, _self);

                    healthable.TakeDamage(damage);
                }
            }

            public void PhysicsTick(float deltaTime)
            {
                _tickHelper.AdvanceTime(deltaTime);
                _trigger.PhysicsTick();
                _tickHelper.AdvanceTicks();
            }

            public bool IsDone
            {
                get { return _tickHelper.DoneTicking; }
            }
        }


        public override void Initialize(GameObject go)
        {
            _curseInstances = new List<CurseInstance>();
            _self = go;
            _magicable = go.GetComponent<IMagicable>();
        }

        public override void Trigger()
        {
            RaycastHit hit;
            if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)) return;

            if ((hit.point - _self.transform.position).sqrMagnitude > _castRange * _castRange)
                return;


            if (_magicable.ManaPoints < _manaCost)
                return;

            _magicable.ModifyMana(-_manaCost, _self);
            _curseInstances.Add(new CurseInstance(this, hit.point));
        }

        public override void PhysicsTick(float deltaTick)
        {
            for (var i = 0; i < _curseInstances.Count; i++)
            {
                var inst = _curseInstances[i];
                if (inst.IsDone)
                {
                    inst.TerminateDebugInstance();
                    _curseInstances.RemoveAt(i);
                    i--;
                }
                else
                {
                    inst.PhysicsTick(deltaTick);
                }
            }
        }
    }
}