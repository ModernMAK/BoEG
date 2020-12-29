using System.Collections.Generic;
using Entity.Abilities.FlameWitch;
using Framework.Ability;
using Framework.Core;
using Framework.Core.Modules;
using Framework.Types;
using UnityEngine;

namespace Entity.Abilities.DarkHeart
{
    [CreateAssetMenu(menuName = "Ability/DarkHeart/Nightmare")]
    public class Nightmare : AbilityObject, IObjectTargetAbility<Actor>, IListener<IStepableEvent>
    {
#pragma warning disable 0649
        [SerializeField] private float _manaCost;
        [SerializeField] private float _castRange;
        [SerializeField] private float _tickInterval;
        [SerializeField] private int _tickCount;
        [SerializeField] private float _tickDamage;
        private List<TickAction> _ticks;
#pragma warning restore 0649

        public override void Initialize(Actor actor)
        {
            base.Initialize(actor);
            _commonAbilityInfo.ManaCost = _manaCost;
            _commonAbilityInfo.Range = _castRange;
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
            if (!AbilityHelper.HasAllComponents(actor.gameObject, typeof(IDamageTarget)))
                return;
            if (!_commonAbilityInfo.TrySpendMana())
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
            _ticks.Add(tickWrapper);
            _commonAbilityInfo.NotifySpellCast();
        }
    }
}