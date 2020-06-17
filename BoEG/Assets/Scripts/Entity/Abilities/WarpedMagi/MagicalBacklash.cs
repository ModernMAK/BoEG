using Entity.Abilities.FlameWitch;
using Framework.Core;
using Framework.Core.Modules;
using Framework.Types;
using UnityEngine;

namespace Entity.Abilities.WarpedMagi
{
    [CreateAssetMenu(menuName = "Ability/WarpedMagi/MagicalBacklash")]
    public class MagicalBacklash : AbilityObject, IStepable
    {
        [SerializeField] private float _damagePerManaSpent;
        [SerializeField] private float _aoeRange;
        private TriggerHelper<SphereCollider> _sphereCollider;
        private IAbilitiable _abilitiable;


        /* Passive Spell
         * Units who cast spells in an AOE take damage based on manacost.
         */


        public override void Initialize(Actor actor)
        {
            base.Initialize(actor);
            _sphereCollider = TriggerUtility.CreateTrigger<SphereCollider>(Self, "MagicalBackLash Trigger");

            _sphereCollider.Trigger.Enter += OnActorEnter;
            _sphereCollider.Trigger.Exit += OnActorExit;
            actor.AddSteppable(this);
        }

        private void OnActorEnter(object sender, TriggerEventArgs args)
        {
            var go = args.Collider.gameObject;
            if (!go.TryGetComponent<Actor>(out var actor))
                return;
            if (!go.TryGetComponent<IAbilitiable>(out var abilitiable))
                return;
            abilitiable.SpellCasted += OnSpellCast;
        }

        private void OnActorExit(object sender, TriggerEventArgs args)
        {
            var go = args.Collider.gameObject;
            if (!go.TryGetComponent<Actor>(out var actor))
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

        public void PreStep(float deltaTime)
        {
        }

        public void Step(float deltaTime)
        {
        }

        public void PostStep(float deltaTime)
        {
        }

        public void PhysicsStep(float deltaTime)
        {
            _sphereCollider.Collider.radius = _aoeRange;
        }
    }
}