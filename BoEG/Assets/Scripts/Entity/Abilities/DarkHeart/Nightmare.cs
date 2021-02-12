using System.Collections.Generic;
using MobaGame.Framework.Core.Modules.Ability.Helpers;
using MobaGame.Framework.Core;
using MobaGame.Framework.Core.Modules;
using MobaGame.Framework.Core.Modules.Ability;
using MobaGame.Framework.Types;
using MobaGame.FX;
using UnityEngine;

namespace MobaGame.Entity.Abilities.DarkHeart
{
    [CreateAssetMenu(menuName = "Ability/DarkHeart/Nightmare")]
    public class Nightmare : AbilityObject, IReflectableAbility, IListener<IStepableEvent>
    {
        #region Variables
#pragma warning disable 0649
        [SerializeField] private Sprite _icon;
		[SerializeField] private float _manaCost;
        [SerializeField] private float _castRange;
        [SerializeField] private float _tickInterval;
        [SerializeField] private int _tickCount;
        [SerializeField] private float _tickDamage;
        [SerializeField] private float _cooldownDuration;
        [SerializeField] private GameObject _nightmareFX;
#pragma warning restore 0649
        #endregion
        private List<TickAction> Ticks { get; set; }            
        private AbilityPredicateBuilder CheckBuilder { get; set; }
        private SimpleAbilityView View { get; set; }

        public override void Initialize(Actor data)
        {
            base.Initialize(data);
            Ticks = new List<TickAction>();
            CheckBuilder = new AbilityPredicateBuilder(data)
            {
                AllowSelf = false,
                Teamable = TeamableChecker.NonAllyOnly(Modules.Teamable),
                Cooldown = new Cooldown(_cooldownDuration),
                CastRange = new CastRange(data.transform) { MaxDistance = _castRange },
                MagicCost = new MagicCost(Modules.Magicable, _manaCost),
            };
            View = new SimpleAbilityView
            {
                Cooldown = CheckBuilder.Cooldown,
                StatCost = CheckBuilder.MagicCost,
                Icon = _icon,
            };
            CheckBuilder.RebuildChecks();
            Register(data);
        }

        public override void ConfirmCast()
        {
            var ray = AbilityHelper.GetScreenRay();
            if (!AbilityHelper.TryGetEntity(ray, out var hit))
                return;
            if (!AbilityHelper.TryGetActor(hit.collider, out var actor))
                return;


            if (!CheckBuilder.AllowCast())
                return;
            if (!CheckBuilder.AllowTarget(actor))
                return;

            if (actor.TryGetModule<ITargetable>(out var targetable))
                if (!targetable.AllowSpellTargets)
                    return;
            if (!AbilityHelper.HasModule<IDamageable>(actor.gameObject))
                return;

            CheckBuilder.DoCast();

            Cast(Self,actor);
            Modules.Abilitiable.NotifyAbilityCast(new AbilityEventArgs(Self, Cost));
        }

        private void OnStep(float deltaTime)
        {
            Ticks.AdvanceAllAndRemoveDone(deltaTime);
        }

        public void Register(IStepableEvent source)
        {
            source.Step += OnStep;
            CheckBuilder.Cooldown.Register(source);
        }

        public void Unregister(IStepableEvent source)
        {
            source.Step += OnStep;
            CheckBuilder.Cooldown.Unregister(source);
        }

        private void Cast(Actor caster, Actor target)
        {
            //Deal damage
            var damagePerTick = new Damage(_tickDamage, DamageType.Magical, DamageFlags.Ability);
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
                Ticks.Remove(tickWrapper);
            }


            Ticks.Add(tickWrapper);
            ApplyFX(target.transform, _tickInterval * _tickCount);
        }
        public void CastReflected(Actor caster) => Cast(caster, Self);

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

    }

}