using System;
using System.Collections.Generic;
using Framework.Core;
using Framework.Core.Modules;
using Framework.Types;
using Triggers;
using UnityEngine;

namespace Entity.Abilities.FlameWitch
{
    /* Unit-Target Spell
        * Applies DOT
        * Deals Damage on Cast
        *
        * When OverHeating;
        *     Eemy heroes in an AOE also recieve DOT
        */


    [CreateAssetMenu(menuName = "Ability/FlameWitch/Ignite")]
    public class Ignite : AbilityObject, IStepable
    {
        [Header("Cast Range")] [SerializeField]
        private float _castRange = 5f;

        [Header("Initial Damage")] [SerializeField]
        private float _damage = 100f;

        private IMagicable _magicable;

        [Header("Mana Cost")] [SerializeField] private float _manaCost = 100f;
        private Overheat _overheatAbility;

        [Header("OverHeat FX")] [SerializeField]
        private float _overheatSearchRange = 1f;

        [SerializeField] private int _tickCount;

        [SerializeField] private float _tickDamage;

        [Header("Damage Over Time")] [SerializeField]
        private float _tickInterval;

        private List<TickAction> _ticks;
        private bool IsInOverheat => _overheatAbility != null && _overheatAbility.IsActive;

        public void PreStep(float deltaTime)
        {
        }

        public void Step(float deltaTime)
        {
            for (var i = 0; i < _ticks.Count; i++)
                if (_ticks[i].Advance(deltaTime))
                {
                    _ticks.RemoveAt(i);
                    i--;
                }
        }

        public void PostStep(float deltaTime)
        {
        }

        public void PhysicsStep(float deltaTime)
        {
        }

        public override void Initialize(Actor actor)
        {
            base.Initialize(actor);
            _magicable = actor.GetComponent<IMagicable>();
            var abilitiable = actor.GetComponent<IAbilitiable>();
            if (abilitiable != null)
                abilitiable.FindAbility(out _overheatAbility);
            //Manually inject the ability as a stepable
            actor.AddSteppable(this);
            _ticks = new List<TickAction>();
        }


        public override void ConfirmCast()
        {
            Cast();
        }

        protected void Cast()
        {
            var ray = AbilityHelper.GetScreenRay();
            if (!Physics.Raycast(ray, out var hit)) //    , 100f, (int) LayerMaskHelper.Entity))
                return;

            if (!AbilityHelper.InRange(Self.transform, hit.point, _castRange))
                return;

            var actor = AbilityHelper.GetActor(hit);
            if (actor == null)
                return;

            if (!AbilityHelper.CanSpendMana(_magicable, _manaCost))
                return;


            AbilityHelper.SpendMana(_magicable, _manaCost);
            SpellLogic(actor);
        }


        private void SpellLogic(Actor target)
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
                    var actor = collider.GetComponent<Actor>();
                    if (actor == target) //Already added
                        continue;
                    if (actor == null) //Not an actor
                        continue;
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
                }
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

        public override float GetManaCost()
        {
            return _manaCost;
        }
    }
}