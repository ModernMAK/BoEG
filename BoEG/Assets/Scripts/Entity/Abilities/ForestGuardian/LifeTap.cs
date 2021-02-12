using MobaGame.Framework.Core;
using MobaGame.Framework.Core.Modules;
using MobaGame.Framework.Core.Modules.Ability;
using MobaGame.Framework.Core.Modules.Ability.Helpers;
using MobaGame.Framework.Types;
using TMPro;
using UnityEngine;

namespace MobaGame.Entity.Abilities.ForestGuardian
{
    [CreateAssetMenu(menuName = "Ability/ForestGuardian/Life Tap")]
    public class LifeTap : AbilityObject, IListener<IStepableEvent>
    {
#pragma warning disable 0649
        private BlackRoseCurse _blackRose;
        [SerializeField]
        private Sprite _rrIcon;
        [SerializeField] private Sprite _brIcon;

        
        [Header("Black Rose")] [SerializeField]
        private float _brRange;
        [SerializeField] private float _brDamage;
        [SerializeField] private float _brSelfHeal;
        [SerializeField] private float _brManaCost;
        [SerializeField] private float _brCooldown;

        [Header("Red Rose")] [SerializeField] private float _rrRange;
        [SerializeField] private float _rrHeal;
        [SerializeField] private float _rrManaCost;
        [SerializeField] private float _rrCooldown;
#pragma warning restore 0649

        public bool IsBlackRose => _blackRose.Active;
        public float CastRange => IsBlackRose ? _brRange : _rrRange;


        public override IAbilityView GetAbilityView() => View;
        public SimpleAbilityView View { get; set; }

        public AbilityPredicateBuilder CheckBuilder { get; set; }
        private TeamableChecker _rrTeamChecker;
        private TeamableChecker _brTeamChecker;
        private DualCooldown _cooldown;

        public override void Initialize(Actor data)
        {
            base.Initialize(data);
            Modules.Abilitiable.TryGetAbility(out _blackRose);
            _cooldown = new DualCooldown(_rrCooldown, _brCooldown);
            _rrTeamChecker = TeamableChecker.AllyOnly(Modules.Teamable);
            _brTeamChecker = TeamableChecker.NonAllyOnly(Modules.Teamable);
            CheckBuilder = new AbilityPredicateBuilder(data)
            {
                Cooldown = _cooldown,
                MagicCost = new MagicCost(Modules.Magicable, _rrManaCost),
                CastRange = new CastRange(data.transform) {MaxDistance = _rrRange},
                AllowSelf = false
            };
            View = new SimpleAbilityView()
            {
                Icon = _rrIcon,
                Cooldown = CheckBuilder.Cooldown,
                StatCost = CheckBuilder.MagicCost,
            };
            CheckBuilder.RebuildChecks();
            Register(data);
        }

        public override void Setup()
        {
            _blackRose.Toggled += BlackRoseOnToggled;
        }

        private void BlackRoseOnToggled(object sender, ChangedEventArgs<bool> e)
        {
            var isBlackRose = e.After;
            _cooldown.SetTimer(isBlackRose);
            CheckBuilder.Teamable = isBlackRose ? _brTeamChecker : _rrTeamChecker;
            CheckBuilder.MagicCost.Cost = isBlackRose ? _brManaCost : _rrManaCost;
            CheckBuilder.CastRange.MaxDistance = isBlackRose ? _brRange : _rrRange;
            CheckBuilder.RebuildChecks();
            View.Icon = isBlackRose ? _brIcon : _rrIcon;
        }

        public override void ConfirmCast()
        {
            if (!AbilityHelper.TryRaycastActor( out var actor))
                return;
            if(CheckBuilder.AllowCast())
                return;
            if(CheckBuilder.AllowTarget(actor))
                return;


            if (IsBlackRose)
            {
                if (actor.TryGetModule<IDamageable>(out var damageable))
                {
                    CheckBuilder.DoCast();
                    CastBlackRoseLifeTap(damageable);
                    CheckBuilder.Cooldown.Begin();
                    var args = new AbilityEventArgs(Self,CheckBuilder.MagicCost.Cost);
                    Modules.Abilitiable.NotifyAbilityCast(args);
                }
            }
            else
            {
                if (actor.TryGetModule<IHealable>(out var healable))
                {
                    CheckBuilder.DoCast();
                    CastRedRoseLifeTap(healable);
                    CheckBuilder.Cooldown.Begin();
                    var args = new AbilityEventArgs(Self,CheckBuilder.MagicCost.Cost);
                    Modules.Abilitiable.NotifyAbilityCast(args);

                }
            }
        }

        void CastBlackRoseLifeTap(IDamageable damageable)
        {
            var dmgVal = new Damage(_brDamage, DamageType.Magical, DamageFlags.Ability);
            var damage = new SourcedDamage(Self, dmgVal);
            damageable.TakeDamage(damage);
            var selfHeal = new SourcedHeal(Self, _brSelfHeal);
            Modules.Healable.Heal(selfHeal);
        }

        void CastRedRoseLifeTap(IHealable healable)
        {
            var heal = new SourcedHeal(Self, _rrHeal);
            healable.Heal(heal);
        }

        public void Register(IStepableEvent source)
        {
            CheckBuilder.Cooldown.Register(source);
        }


        public void Unregister(IStepableEvent source)
        {
            CheckBuilder.Cooldown.Unregister(source);
        }
    }
}