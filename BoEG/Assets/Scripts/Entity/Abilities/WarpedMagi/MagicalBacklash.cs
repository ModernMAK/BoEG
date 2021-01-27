using System.Collections.Generic;
using MobaGame.Framework.Core;
using MobaGame.Framework.Core.Modules;
using MobaGame.Framework.Core.Modules.Ability;
using MobaGame.Framework.Core.Trigger;
using MobaGame.Framework.Types;
using UnityEngine;

namespace MobaGame.Entity.Abilities.WarpedMagi
{
    [CreateAssetMenu(menuName = "Ability/WarpedMagi/MagicalBacklash")]
    public class MagicalBacklash : AbilityObject, IListener<IStepableEvent>
    {
#pragma warning disable 0649
        [SerializeField] private float _damagePerManaSpent;
        [SerializeField] private float _aoeRange;
        private TriggerHelper<SphereCollider> _sphereCollider;
        private List<IAbilitiable> _targetBuffer;
#pragma warning restore 0649


        /* Passive Spell
         * Units who cast spells in an AOE take damage based on manacost.
         */

        private void ClearTargets()
        {
            foreach (var target in _targetBuffer)
                target.SpellCasted -= OnSpellCast;
            _targetBuffer.Clear();
        }


        public override void Initialize(Actor actor)
        {
            base.Initialize(actor);
            _sphereCollider = TriggerUtility.CreateTrigger<SphereCollider>(Self, "MagicalBacklash Trigger");
            _sphereCollider.Trigger.Enter += OnActorEnter;
            _sphereCollider.Trigger.Exit += OnActorExit;
            _targetBuffer = new List<IAbilitiable>();
            Register(actor);
            if(actor.TryGetModule<IKillable>(out var killable))
                killable.Died += SelfDied;
        }

        private void SelfDied(object sender, DeathEventArgs e)
        {
            ClearTargets();
        }

        private void OnActorEnter(object sender, TriggerEventArgs args)
        {
            var collider = args.Collider;
            if (!AbilityHelper.TryGetActor(collider, out var actor))
                return;
            if (IsSelf(actor))
                return;
            if (!actor.TryGetModule<IAbilitiable>(out var abilitiable))
                return;
            abilitiable.SpellCasted += OnSpellCast;
            _targetBuffer.Add(abilitiable);
        }

        private void OnActorExit(object sender, TriggerEventArgs args)
        {
            var collider = args.Collider;
            if (!AbilityHelper.TryGetActor(collider, out var actor))
                return;
            if (!actor.TryGetModule<IAbilitiable>(out var abilitiable))
                return;
            abilitiable.SpellCasted -= OnSpellCast;
            _targetBuffer.Remove(abilitiable);
        }


        private void OnSpellCast(object sender, SpellEventArgs args)
        {
            var caster = args.Caster;
            if (caster.TryGetModule<ITeamable>(out var teamable))
                if (Modules.Teamable?.SameTeam(teamable) ?? false)
                    return;

            if (!caster.TryGetModule<IDamageTarget>(out var damageTarget))
                return;

            var damageValue = _damagePerManaSpent * args.ManaSpent;
            damageValue = Mathf.Max(damageValue, 0f);
            var damage = new Damage(damageValue, DamageType.Pure, DamageModifiers.Ability);
            damageTarget.TakeDamage(Self, damage);
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