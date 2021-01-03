using System.Collections.Generic;
using Framework.Ability;
using Framework.Core;
using Framework.Core.Modules;
using Framework.Types;
using UnityEngine;

namespace Entity.Abilities.DarkHeart
{
    [CreateAssetMenu(menuName = "Ability/DarkHeart/Nightmare")]
    public class Nightmare : AbilityObject, IObjectTargetAbility<Actor>, IListener<IStepableEvent>, IStatCostAbility
    {
#pragma warning disable 0649
        [SerializeField] private float _manaCost;
        [SerializeField] private float _castRange;
        [SerializeField] private float _tickInterval;
        [SerializeField] private int _tickCount;
        [SerializeField] private float _tickDamage;
        private List<TickAction> _ticks;
        [SerializeField] private GameObject _nightmareFX;

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

        public override void Initialize(Actor actor)
        {
            base.Initialize(actor);
            _ticks = new List<TickAction>();
            Register(actor);
        }

        public override void ConfirmCast()
        {
            var ray = AbilityHelper.GetScreenRay();
            if (!AbilityHelper.TryGetEntity(ray, out var hit))
                return;
            if (!AbilityHelper.TryGetActor(hit.collider, out var actor))
                return;
            if (!AbilityHelper.InRange(Self.transform,actor.transform,_castRange))
                return;
            if (!AbilityHelper.HasAllComponents(actor.gameObject, typeof(IDamageTarget)))
                return;
            if (!AbilityHelper.TrySpendMagic(this, Modules.Magicable)) 
                return;
            CastObjectTarget(actor);
        }

        private void OnStep(float deltaTime)
        {
            _ticks.AdvanceAllAndRemoveDone(deltaTime);
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
            var damageable = target.GetComponent<IDamageTarget>();

            
            
            void InternalTick()
            {
                damageable.TakeDamage(Self.gameObject, damagePerTick);
            }

            var tickWrapper = new TickAction
            {
                Callback = InternalTick,
                TickCount = _tickCount,
                TickInterval = _tickInterval
            };

            if (target.TryGetComponent<IHealthable>(out var healthable))
                healthable.Died += RemoveTick;

            void RemoveTick(object sender, DeathEventArgs args)
            {
                healthable.Died -= RemoveTick;
                _ticks.Remove(tickWrapper);
            }

            
            _ticks.Add(tickWrapper);
            ApplyFX(target.transform, _tickInterval * _tickCount);
            Modules.Abilitiable.NotifySpellCast(new SpellEventArgs(){Caster = Self,ManaSpent = Cost});
        }

        public float Cost => _manaCost;

        public bool CanSpendCost() => Modules.Magicable.HasMagic(Cost);
    }
}