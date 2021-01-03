using System;
using Framework.Ability;
using Framework.Core;
using Framework.Core.Modules;
using Framework.Types;
using Modules.Teamable;
using Triggers;
using UnityEngine;

namespace Entity.Abilities.FlameWitch
{
    [CreateAssetMenu(menuName = "Ability/FlameWitch/FlashFire")]
    public class FlashFire : AbilityObject, INoTargetAbility, IStatCostAbility, IListener<IStepableEvent>,
        IToggleableAbility, ICooldownAbility
    {
        /* Channeling Spell
         * Deals damage in an AOE around FlameWitch
         * Travels further based on channel duration.
         * When Overheating;
         *     Channel does not prevent movement.
         */

        public override void Initialize(Actor actor)
        {
            base.Initialize(actor);
            _isActive = false;
            Register(actor);
            _channelTimer = new DurationTimer(_channelTime);
            _cooldownTimer = new DurationTimer(_cooldown, true);
            Modules.Abilitiable.FindAbility<Overheat>(out _overheatAbility);
        }

#pragma warning disable 0649
        [Header("Channel Time")] [SerializeField]
        private float _channelTime;

        private DurationTimer _channelTimer;

        [Header("Mana Cost")] [SerializeField] private float _manaCost;

        [Header("Cooldown")] [SerializeField] private float _cooldown;
        private DurationTimer _cooldownTimer;

        [Header("Area Of Effect")] [SerializeField]
        private float _aoeSearchRange;

        [SerializeField] private float _aoeDamage;


        private Overheat _overheatAbility;
#pragma warning restore 0649


        public override void ConfirmCast()
        {
            if (Active)
                return;

            if (!_cooldownTimer.Done)
                return;

            if (!Modules.Magicable.TrySpendMagic(Cost))
                return;

            _isActive = true;
            _channelTimer.Reset();
            CastNoTarget();
            Modules.Abilitiable.NotifySpellCast(new SpellEventArgs() {Caster = Self, ManaSpent = Cost});
        }

        public void CastNoTarget()
        {
            //TODO channel

            var targets = Physics.OverlapSphere(Self.transform.position, _aoeSearchRange, (int) LayerMaskHelper.Entity);

            var damage = new Damage(_aoeDamage, DamageType.Magical, DamageModifiers.Ability);
            foreach (var target in targets)
            {
                if (!target.TryGetComponent<Actor>(out var actor))
                    continue;
                if (actor == Self)
                    continue;

                if (!target.TryGetComponent<IDamageTarget>(out var damageTarget))
                    continue;
                if (AbilityHelper.SameTeam(Modules.Teamable, target))
                    continue;

                damageTarget.TakeDamage(Self.gameObject, damage);
            }
        }

        public float Cost => _manaCost;

        public bool CanSpendCost() => Modules.Magicable.HasMagic(Cost);

        public void Register(IStepableEvent source)
        {
            source.PreStep += OnPreStep;
        }

        private void OnPreStep(float deltaTime)
        {
            if (Active)
                if (_channelTimer.AdvanceTimeIfNotDone(deltaTime))
                {
                    _isActive = false;
                    _cooldownTimer.Reset();
                }

            if (!Active)
            {
                _cooldownTimer.AdvanceTimeIfNotDone(deltaTime);
            }
        }

        public void Unregister(IStepableEvent source)
        {
            source.PreStep -= OnPreStep;
        }

        private bool _isActive;
        public bool Active => _isActive;
        public float Cooldown => _cooldownTimer.Duration;
        public float CooldownRemaining => _cooldownTimer.RemainingTime;
        public float CooldownNormal => _cooldownTimer.ElapsedTime / _cooldownTimer.Duration;
    }
}