using System;
using System.Collections.Generic;
using Framework.Ability;
using Framework.Core;
using Framework.Core.Modules;
using Framework.Types;
using Triggers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Entity.Abilities.FlameWitch
{
    /* Unit-Target Spell
        * Applies DOT
        * Deals Damage on Cast
        *
        * When OverHeating;
        *     Enemy heroes in an AOE also recieve DOT
        */


    [CreateAssetMenu(menuName = "Ability/FlameWitch/Ignite")]
    public class Ignite : AbilityObject, IListener<IStepableEvent>, IObjectTargetAbility<Actor>, IStatCostAbility
    {
#pragma warning disable 0649
        [Header("Cast Range")] [SerializeField]
        private float _castRange = 5f;

        [Header("Initial Damage")] [SerializeField]
        private float _damage = 100f;


        [Header("Mana Cost")] [SerializeField] private float _manaCost = 100f;

        [Header("OverHeat FX")] [SerializeField]
        private float _overheatSearchRange = 1f;

        [Header("Damage Over Time")] [SerializeField]
        private float _tickInterval;

        [SerializeField] private int _tickCount;

        [SerializeField] private float _tickDamage;


        private Overheat _overheatAbility;

        private List<TickAction> _ticks;

        [SerializeField] private GameObject _igniteFX;
#pragma warning restore 0649

        private void ApplyFX(Transform target, float duration)
        {
            if (_igniteFX == null)
                return;
            var instance = Instantiate(_igniteFX, target.position, Quaternion.identity);
            if (!instance.TryGetComponent<DieAfterDuration>(out var die))
                die = instance.AddComponent<DieAfterDuration>();
            if (!instance.TryGetComponent<FollowTarget>(out var follow))
                follow = instance.AddComponent<FollowTarget>();
            if (instance.TryGetComponent<ParticleSystem>(out var ps))
                ps.Play();
            follow.SetTarget(target);
            die.SetDuration(duration);
            die.StartTimer();
        }

        private bool IsInOverheat => _overheatAbility != null && _overheatAbility.IsActive;

        public void OnStep(float deltaTime)
        {
            _ticks.AdvanceAllAndRemoveDone(deltaTime);
        }

        public void CastObjectTarget(Actor target)
        {
            //Deal damage
            var damage = new Damage(_damage, DamageType.Magical, DamageModifiers.Ability);
//            var targetable = target.GetComponent<ITargetable>();
            var damagable = target.GetComponent<IDamageTarget>();
            damagable.TakeDamage(Self.gameObject, damage);
            //TODO add DOT
            //Gather DOT targets
            var dotTargets = new List<Actor> {target};
            if (IsInOverheat)
            {
                var colliders = Physics.OverlapSphere(target.transform.position, _overheatSearchRange,
                    (int) LayerMaskHelper.Entity);
                foreach (var collider in colliders)
                {
                    if(!AbilityHelper.TryGetActor(collider, out var actor))
                        continue;
                    if (actor == target) //Already added
                        continue;
                    if (AbilityHelper.SameTeam(Modules.Teamable, actor.gameObject))
                        continue; //Skip allies
                    dotTargets.Add(actor);
                }
            }

            foreach (var actor in dotTargets)
                if (GetDotAction(actor, out var action))
                {
                    var tickWrapper = new TickAction
                    {
                        Callback = action,
                        TickCount = _tickCount,
                        TickInterval = _tickInterval
                    };
                    _ticks.Add(tickWrapper);
                    ApplyFX(actor.transform, _tickCount * _tickInterval);
                }

            Modules.Abilitiable.NotifySpellCast(new SpellEventArgs(){Caster = Self, ManaSpent = _manaCost});
        }

        public override void Initialize(Actor actor)
        {
            base.Initialize(actor);

            Modules.Abilitiable.FindAbility(out _overheatAbility);
            //Manually inject the ability as a stepable
            actor.AddSteppable(this);
            _ticks = new List<TickAction>();
        }


        public override void ConfirmCast()
        {
            var ray = AbilityHelper.GetScreenRay();
            if (!AbilityHelper.TryGetEntity(ray, out var hit))
                return;
            if (!AbilityHelper.InRange(Self.transform, hit.point, _castRange))
                return;
            if (!AbilityHelper.TryGetActor(hit.collider, out var actor))
                return;
            if (IsSelf(actor))
                return;
            if (!AbilityHelper.HasAllComponents(actor.gameObject, typeof(IDamageTarget)))
                return;
            if (!AbilityHelper.TrySpendMagic(this, Modules.Magicable))
                return;

            CastObjectTarget(actor);
        }


        private bool GetDotAction(Actor actor, out Action dotAction)
        {
            dotAction = default;
            var dmgTarget = actor.GetComponent<IDamageTarget>();
            if (dmgTarget == null)
                return false;

            var source = Self.gameObject;
            var damage = new Damage(_tickDamage, DamageType.Magical, DamageModifiers.Ability);

            void internalFunc()
            {
                dmgTarget.TakeDamage(source, damage);
            }

            dotAction = internalFunc;
            return true;
        }


        public void Register(IStepableEvent source)
        {
            source.Step += OnStep;
        }

        public void Unregister(IStepableEvent source)
        {
            source.Step -= OnStep;
        }

        public float Cost => _manaCost;

        public bool CanSpendCost() => Modules.Magicable.HasMagic(Cost);
    }
}