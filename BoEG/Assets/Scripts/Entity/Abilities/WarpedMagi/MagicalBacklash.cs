using Entity.Abilities.FlameWitch;
using Framework.Ability;
using Framework.Core;
using Framework.Core.Modules;
using Framework.Types;
using UnityEngine;

namespace Entity.Abilities.WarpedMagi
{
    [CreateAssetMenu(menuName = "Ability/WarpedMagi/MagicalBacklash")]
    public class MagicalBacklash : AbilityObject, IListener<IStepableEvent>
    {
#pragma warning disable 0649
        [SerializeField] private float _damagePerManaSpent;
        [SerializeField] private float _aoeRange;
        private TriggerHelper<SphereCollider> _sphereCollider;
#pragma warning restore 0649


        /* Passive Spell
         * Units who cast spells in an AOE take damage based on manacost.
         */

        public override void Initialize(Actor actor)
        {
            base.Initialize(actor);
            _sphereCollider = TriggerUtility.CreateTrigger<SphereCollider>(Self, "MagicalBacklash Trigger");
            _sphereCollider.Trigger.Enter += OnActorEnter;
            _sphereCollider.Trigger.Exit += OnActorExit;
            actor.AddSteppable(this);
        }

        private void OnActorEnter(object sender, TriggerEventArgs args)
        {
            var go = args.Collider.gameObject;
            if (!go.TryGetComponent<Actor>(out _))
                return;
            if (!go.TryGetComponent<IAbilitiable>(out var abilitiable))
                return;
            abilitiable.SpellCasted += OnSpellCast;
        }

        private void OnActorExit(object sender, TriggerEventArgs args)
        {
            var go = args.Collider.gameObject;
            if (!go.TryGetComponent<Actor>(out _))
                return;
            if (!go.TryGetComponent<IAbilitiable>(out var abilitiable))
                return;
            abilitiable.SpellCasted -= OnSpellCast;
        }


        private void OnSpellCast(object sender, SpellEventArgs args)
        {
            var caster = args.Caster;
            var damageTarget = caster.GetComponent<IDamageTarget>();
            var damageValue = _damagePerManaSpent * args.ManaSpent;
            damageValue = Mathf.Max(damageValue, 0f);
            var damage = new Damage(damageValue, DamageType.Pure, DamageModifiers.Ability);
            damageTarget.TakeDamage(Self.gameObject, damage);
        }

        private void OnPhysicsStep(float deltaTime)
        {
            _sphereCollider.Collider.radius = _aoeRange;
        }

        public void Register(IStepableEvent source)
        {
            source.PhysicsStep += OnPhysicsStep;
        }

        public void Unregister(IStepableEvent source)
        {
            source.PhysicsStep -= OnPhysicsStep;
        }
    }
}