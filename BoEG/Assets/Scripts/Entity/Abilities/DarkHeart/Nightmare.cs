using System.Collections.Generic;
using MobaGame.Framework.Core;
using MobaGame.Framework.Core.Modules;
using MobaGame.Framework.Core.Modules.Ability;
using MobaGame.Framework.Types;
using MobaGame.FX;
using UnityEngine;

namespace MobaGame.Entity.Abilities.DarkHeart
{
    [CreateAssetMenu(menuName = "Ability/DarkHeart/Nightmare")]
    public class Nightmare : AbilityObject, IObjectTargetAbility<Actor>, IListener<IStepableEvent>, IStatCostAbility,
        ICooldownAbility
    {
#pragma warning disable 0649
        [SerializeField] private float _manaCost;
        [SerializeField] private float _castRange;
        [SerializeField] private float _tickInterval;
        [SerializeField] private int _tickCount;
        [SerializeField] private float _tickDamage;
        private List<TickAction> _ticks;
        [SerializeField] private float _cooldown;
        [SerializeField] private GameObject _nightmareFX;
        private DurationTimer _cooldownTimer;
#pragma warning restore 0649

        private void ApplyFX(Transform target, float duration)
        {
            if (_nightmareFX == null)
                return;
            var instance = Instantiate(_nightmareFX, target.position, Quaternion.identity);
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

        public override void Initialize(Actor data)
        {
            base.Initialize(data);
            _ticks = new List<TickAction>();
            _cooldownTimer = new DurationTimer(_cooldown, true);
            Register(data);
        }

        public override void ConfirmCast()
        {
            var ray = AbilityHelper.GetScreenRay();
            if (!_cooldownTimer.Done)
                return;
            if (!AbilityHelper.TryGetEntity(ray, out var hit))
                return;
            if (!AbilityHelper.TryGetActor(hit.collider, out var actor))
                return;
            if (actor == Self)
                return;
            if (!AbilityHelper.InRange(Self.transform, actor.transform, _castRange))
                return;
            if(actor.TryGetModule<ITeamable>(out var teamable))
                if (Modules.Teamable?.SameTeam(teamable) ?? false)
                    return;
            if (actor.TryGetModule<ITargetable>(out var targetable))
                if (!targetable.AllowSpellTargets)
                    return;
            if (!AbilityHelper.HasModule<IDamageable>(actor.gameObject))
                return;
            if (!AbilityHelper.TrySpendMagic(this, Modules.Magicable))
                return;


            _cooldownTimer.Reset();
            CastObjectTarget(actor);
            Modules.Abilitiable.NotifySpellCast(new AbilityEventArgs(Self, Cost));
        }

        private void OnStep(float deltaTime)
        {
            _ticks.AdvanceAllAndRemoveDone(deltaTime);
            _cooldownTimer.AdvanceTimeIfNotDone(deltaTime);
        }

        public void Register(IStepableEvent source)
        {
            source.Step += OnStep;
        }

        public void Unregister(IStepableEvent source)
        {
            source.Step += OnStep;
        }

        public void CastObjectTarget(Actor target)
        {
            //Deal damage
            var damagePerTick = new Damage(_tickDamage, DamageType.Magical, DamageModifiers.Ability);
            var damageable = target.GetModule<IDamageable>();


            void InternalTick()
            {
                damageable.TakeDamage(Self, damagePerTick);
            }

            var tickWrapper = new TickAction
            {
                Callback = InternalTick,
                TickCount = _tickCount,
                TickInterval = _tickInterval
            };

            if (target.TryGetModule<IKillable>(out var killable))
                killable.Died += RemoveTick;

            void RemoveTick(object sender, DeathEventArgs args)
            {
                killable.Died -= RemoveTick;
                _ticks.Remove(tickWrapper);
            }


            _ticks.Add(tickWrapper);
            ApplyFX(target.transform, _tickInterval * _tickCount);
        }

        public float Cost => _manaCost;

        public bool CanSpendCost() => Modules.Magicable.HasMagic(Cost);

        public float Cooldown => _cooldownTimer.Duration;

        public float CooldownRemaining => _cooldownTimer.RemainingTime;

        public float CooldownNormal => _cooldownTimer.ElapsedTime / _cooldownTimer.Duration;
    }
}