using MobaGame.Framework.Core;
using MobaGame.Framework.Core.Modules;
using MobaGame.Framework.Core.Modules.Ability;
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
        private Sprite _rrIcon => _icon;
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


        private SimpleAbilityView _view;
        private DurationTimer _rrCooldownTimer;
        private DurationTimer _brCooldownTimer;
        public override IAbilityView GetAbilityView() => _view;

        public override void Initialize(Actor data)
        {
            base.Initialize(data);
            Modules.Abilitiable.TryGetAbility(out _blackRose);
            _rrCooldownTimer = new DurationTimer(_rrCooldown,true);
            _brCooldownTimer = new DurationTimer(_brCooldown,true);
            _view = new SimpleAbilityView()
            {
                StatCost = new StatCostAbilityView(Modules.Magicable,_rrManaCost),
                Cooldown = new CooldownAbilityView(_rrCooldownTimer),
                Icon = _rrIcon
            };
            Register(data);
        }

        public override void Setup()
        {
            _blackRose.Toggled += BlackRoseOnToggled;
        }

        private void BlackRoseOnToggled(object sender, ChangedEventArgs<bool> e)
        {
            var isBlackRose = e.After;
            _view.Cooldown.Timer = isBlackRose ? _brCooldownTimer : _rrCooldownTimer;
            _view.StatCost.Cost = isBlackRose ? _brManaCost : _rrManaCost;
            _view.Icon = isBlackRose ? _brIcon : _rrIcon;
        }

        public override void ConfirmCast()
        {
            var ray = AbilityHelper.GetScreenRay();
            if (!AbilityHelper.TryGetEntity(ray, out var hit))
                return;
            if (!AbilityHelper.TryGetActor(hit.collider, out var actor))
                return;
            if(actor == Self)
                return;
            if(_view.Cooldown.OnCooldown)
                return;
            if (Modules.Teamable != null)
            {
                var teamable = Modules.Teamable;
                if (actor.TryGetModule<ITeamable>(out var otherTeamable))
                {
                    //To explain this 'BR' is black rose
                    //BR && Ally => Disallow (dont hurt allies)
                    //BR && !Ally => Allow (hurt enemies, heal self)
                    //!BR && Ally => Allow (heal ally)
                    //!BR && !Ally => Disallow (dont heal enemies/neutrals)
                    var isAlly = teamable.IsRelation(otherTeamable, TeamRelation.Ally);
                    if (!(IsBlackRose ^ isAlly))
                        return;
                }
            }

            if (IsBlackRose)
            {
                if (actor.TryGetModule<IDamageable>(out var damageable))
                {
                    if (_view.StatCost.TrySpendCost())
                    {
                        CastBlackRoseLifeTap(damageable);
                        _view.Cooldown.StartCooldown();
                        var args = new AbilityEventArgs(Self,_view.StatCost.Cost);
                        Modules.Abilitiable.NotifyAbilityCast(args);
                    }
                }
            }
            else
            {
                if (actor.TryGetModule<IHealable>(out var healable))
                {
                    if (_view.StatCost.TrySpendCost())
                    {
                        CastRedRoseLifeTap(healable);
                        _view.Cooldown.StartCooldown();
                        var args = new AbilityEventArgs(Self,_view.StatCost.Cost);
                        Modules.Abilitiable.NotifyAbilityCast(args);
                    }

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
            source.PreStep += OnPreStep;
        }

        private void OnPreStep(float deltaTime)
        {
            _rrCooldownTimer.AdvanceTimeIfNotDone(deltaTime);
            _brCooldownTimer.AdvanceTimeIfNotDone(deltaTime);
        }

        public void Unregister(IStepableEvent source)
        {
            source.PreStep -= OnPreStep;
        }
    }
}