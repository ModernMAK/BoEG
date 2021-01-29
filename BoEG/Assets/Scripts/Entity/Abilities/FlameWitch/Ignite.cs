using System.Collections.Generic;
using MobaGame.Framework.Core;
using MobaGame.Framework.Core.Modules;
using MobaGame.Framework.Core.Modules.Ability;
using MobaGame.Framework.Types;
using MobaGame.FX;
using UnityEngine;

namespace MobaGame.Entity.Abilities.FlameWitch
{
    /* Unit-Target Spell
        * Applies DOT
        * Deals Damage on Cast
        *
        * When OverHeating;
        *     Enemy heroes in an AOE also recieve DOT
        */


    [CreateAssetMenu(menuName = "Ability/FlameWitch/Ignite")]
    public class Ignite : AbilityObject, IListener<IStepableEvent>, IObjectTargetAbility<Actor>, IStatCostAbility,
        ICooldownAbility
    {
#pragma warning disable 0649
        [Header("Cast Range")] [SerializeField]
        private float _castRange = 5f;

        [Header("Initial Damage")] [SerializeField]
        private float _damage = 100f;


        [Header("Cooldown")] [SerializeField] private float _cooldown;
        private DurationTimer _cooldownTimer;

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
            var damagable = target.GetModule<IDamageTarget>();
            damagable.TakeDamage(Self, damage);
            //Gather DOT targets
            var dotTargets = new List<Actor> {target};
            if (IsInOverheat)
            {
                var colliders = Physics.OverlapSphere(target.transform.position, _overheatSearchRange,
                    (int) LayerMaskHelper.Entity);
                foreach (var collider in colliders)
                {
                    if (!AbilityHelper.TryGetActor(collider, out var actor))
                        continue;
                    if (IsSelf(actor))
                        continue;
                    if (actor == target) //Already added
                        continue;
                    if (AbilityHelper.SameTeam(Modules.Teamable, actor))
                        continue; //Skip allies
                    dotTargets.Add(actor);
                }
            }

            foreach (var actor in dotTargets)
            {
                if (!actor.TryGetModule<IDamageTarget>(out var damageTarget))
                    continue;
                var source = Self;
                var dotDamage = new Damage(_tickDamage, DamageType.Magical, DamageModifiers.Ability);

                void internalFunc()
                {
                    damageTarget.TakeDamage(source, dotDamage);
                }

                var tickWrapper = new TickAction
                {
                    Callback = internalFunc,
                    TickCount = _tickCount,
                    TickInterval = _tickInterval
                };

                if (!actor.TryGetModule<IKillable>(out var killable))
                    killable.Died += RemoveTick;

                void RemoveTick(object sender, DeathEventArgs args)
                {
                    killable.Died -= RemoveTick;
                    _ticks.Remove(tickWrapper);
                }


                _ticks.Add(tickWrapper);
                ApplyFX(actor.transform, _tickCount * _tickInterval);
            }
        }

        public override void Initialize(Actor data)
        {
            base.Initialize(data);
            Modules.Abilitiable.TryGetAbility(out _overheatAbility);
            _cooldownTimer = new DurationTimer(_cooldown, true);
            //Manually inject the ability as a stepable
            data.AddSteppable(this);
            _ticks = new List<TickAction>();
        }

        public override void ConfirmCast()
        {
            if (!_cooldownTimer.Done)
                return;

            var ray = AbilityHelper.GetScreenRay();
            if (!AbilityHelper.TryGetEntity(ray, out var hit))
                return;

            if (!AbilityHelper.TryGetActor(hit.collider, out var actor))
                return;

            if (IsSelf(actor))
                return;

            if (!AbilityHelper.InRange(Self.transform, actor.transform.position, _castRange))
                return;

            if (AbilityHelper.SameTeam(Modules.Teamable, actor))
                return;

            if (!AbilityHelper.AllowSpellTargets(actor))
                return;

            if (!AbilityHelper.HasModule<IDamageTarget>(actor.gameObject))
                return;


            if (!AbilityHelper.TrySpendMagic(this, Modules.Magicable))
                return;

            _cooldownTimer.Reset();
            CastObjectTarget(actor);
            Modules.Abilitiable.NotifySpellCast(new SpellEventArgs() {Caster = Self, ManaSpent = _manaCost});
        }


        public void Register(IStepableEvent source)
        {
            source.PreStep += OnPreStep;
            source.Step += OnStep;
        }

        private void OnPreStep(float deltaTime)
        {
            _cooldownTimer.AdvanceTimeIfNotDone(deltaTime);
        }

        public void Unregister(IStepableEvent source)
        {
            source.PreStep -= OnPreStep;
            source.Step -= OnStep;
        }

        public float Cost => _manaCost;

        public bool CanSpendCost() => Modules.Magicable.HasMagic(Cost);

        public float Cooldown => _cooldownTimer.Duration;
        public float CooldownRemaining => _cooldownTimer.RemainingTime;
        public float CooldownNormal => _cooldownTimer.ElapsedTime / _cooldownTimer.Duration;
    }
}