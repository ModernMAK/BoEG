using Framework.Core;
using MobaGame.Framework.Core;
using MobaGame.Framework.Core.Modules;
using MobaGame.Framework.Core.Modules.Ability;
using MobaGame.Framework.Core.Modules.Ability.Helpers;
using MobaGame.Framework.Types;
using System;
using UnityEngine;

namespace MobaGame.Entity.Abilities.FlameWitch
{
    [CreateAssetMenu(menuName = "Ability/FlameWitch/FlashFire")]
    public class FlashFire : AbilityObject, IListener<IStepableEvent>, IToggleable
    {
        /* Channeling Spell
         * Deals damage in an AOE around FlameWitch
         * Travels further based on channel duration.
         * When Overheating;
         *     Channel does not prevent movement.
         */

        private AbilityPredicateBuilder CheckBuilder { get; set; }
        private SimpleAbilityView View { get; set; }
        public override void Initialize(Actor data)
        {
            base.Initialize(data);
            _channelTimer = new DurationTimer(_channelTime);
            Modules.Abilitiable.TryGetAbility(out _overheatAbility);
            CheckBuilder = new AbilityPredicateBuilder(data)
            {
                Teamable = TeamableChecker.NonAllyOnly(Modules.Teamable),
                MagicCost = new MagicCost(Modules.Magicable, _manaCost),
                Cooldown = new Cooldown(_cooldown),
                AllowSelf = false,
            };
            View = new SimpleAbilityView()
            {
                Icon = _icon,
                Cooldown = CheckBuilder.Cooldown,
                StatCost = CheckBuilder.MagicCost,
                Toggleable = new ToggleableAbilityView(){ShowActive = true}
            };
            CheckBuilder.RebuildChecks();
            Register(data);
                
        }

#pragma warning disable 0649
        [SerializeField] private Sprite _icon;
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

            if (!CheckBuilder.AllowCast())
                return;

            Active = true;
            _channelTimer.Reset();
            CheckBuilder.MagicCost.SpendCost();
            CastNoTarget();
            Modules.Abilitiable.NotifyAbilityCast(new AbilityEventArgs(Self, CheckBuilder.MagicCost.Cost));
        }

        public override IAbilityView GetAbilityView() => View;

        public void CastNoTarget()
        {
            //TODO channel

            var targets = Physics.OverlapSphere(Self.transform.position, _aoeSearchRange, (int) LayerMaskFlag.Entity);

            var damage = new Damage(_aoeDamage, DamageType.Magical, DamageFlags.Ability);
            foreach (var target in targets)
            {
                if (!AbilityHelper.TryGetActor(target, out var actor))
                    continue;
                if (!CheckBuilder.AllowTarget(actor))
                    continue;

                if (!actor.TryGetModule<IDamageable>(out var damageTarget))
                    continue;

                damageTarget.TakeDamage(Self, damage);
            }
        }

        public void Register(IStepableEvent source)
        {
            source.PreStep += OnPreStep;
        }

        private void OnPreStep(float deltaTime)
        {
            if (Active)
                if (_channelTimer.AdvanceTimeIfNotDone(deltaTime))
                {
                    Active = false;
                    CheckBuilder.Cooldown.Begin();
                }

            if (!Active)
            {
                CheckBuilder.Cooldown.Advance(deltaTime);
            }
        }

        public void Unregister(IStepableEvent source)
        {
            source.PreStep -= OnPreStep;
        }


        public bool Active {
            get => View.Toggleable.Active;
            set => View.Toggleable.Active = value;
        } 


        event EventHandler<ChangedEventArgs<bool>> IToggleable.Toggled
        {
            add => View.Toggleable.Toggled += value;
            remove => View.Toggleable.Toggled -= value;
        }
    }
}