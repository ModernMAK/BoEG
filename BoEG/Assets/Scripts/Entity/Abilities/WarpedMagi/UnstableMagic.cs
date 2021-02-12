using System.Collections.Generic;
using MobaGame.Framework.Core;
using MobaGame.Framework.Core.Modules;
using MobaGame.Framework.Core.Modules.Ability;
using MobaGame.Framework.Core.Modules.Ability.Helpers;
using MobaGame.Framework.Types;
using MobaGame.FX;
using UnityEngine;

namespace MobaGame.Entity.Abilities.WarpedMagi
{
    [CreateAssetMenu(menuName = "Ability/WarpedMagi/UnstableMagic")]
    public class UnstableMagic : AbilityObject, IReflectableAbility, IListener<IStepableEvent>
    {
        // [Header("Mana Cost")]
        /* Ground Target Spell
         * Magical Nuke, jumps between targets. Dealing less damage per jump.
         * Does not target the same target twice. 
         */

#pragma warning disable 0649

        [SerializeField] private float _manaCost;
        [SerializeField] private float _castRange;
        [Header("Damage")] [SerializeField] private float _damage;
        [Header("Jumps")] [SerializeField] private float _searchRange;
        [SerializeField] private int _additionalJumps;

        [Header("Cooldown")] [SerializeField] private float _cooldown;

        [SerializeField] private GameObject _unstableMagicFX;
        private CastRange CastRange { get; set; }

        #region State Variables
        

        #endregion
        
#pragma warning restore 0649

        public override void Initialize(Actor data)
        {
            base.Initialize(data);
            Checker = new AbilityPredicateBuilder(data)
            {
                Cooldown = new Cooldown(_cooldown),
                MagicCost = new MagicCost(Modules.Magicable, _manaCost),
                Teamable = TeamableChecker.NonAllyOnly(Modules.Teamable)
            };
            View = new SimpleAbilityView()
            {
                Cooldown = Checker.Cooldown,
                StatCost = Checker.MagicCost,
            };
            CastRange = new CastRange(data.transform){MaxDistance = _castRange};
            Checker.RebuildChecks();
            Register(data);
        }

        public override void ConfirmCast()
        {
            if(!Checker.AllowCast())
                return;
            var ray = AbilityHelper.GetScreenRay();
            if (!AbilityHelper.TryGetWorldOrEntity(ray, out var hit))
                return;
            
            var unitTarget = AbilityHelper.TryGetActor(hit.collider, out var actor);
            if (actor == Self)
                return;

            if (!unitTarget && !CastRange.InRange(hit.point))
                return;
            if (unitTarget && !CastRange.InRange(actor))
                return;


            Checker.DoCast();
            if (!unitTarget)
                CastGroundTarget(hit.point);
            else
                CastObjectTarget(actor);

            Modules.Abilitiable.NotifyAbilityCast(new AbilityEventArgs(Self, View.StatCost.Cost));
        }

        public void CastGroundTarget(Vector3 worldPos)
        {
            if (TrySearchTarget(worldPos, out var target))
                InternalJumpTarget(target);
        }

        public void CastObjectTarget(Actor target) => InternalJumpTarget(target);

        private void InternalJumpTarget(Actor initialTarget)
        {
            InternalSingleTarget(initialTarget);
            var ignore = new List<Actor>() {initialTarget};
            var prevTarget = initialTarget;
            for (var i = 0; i < _additionalJumps; i++)
            {
                if (!TrySearchTarget(prevTarget.transform.position, ignore, out var nextTarget))
                    break;
                InternalSingleTarget(nextTarget);
                prevTarget = nextTarget;
                ignore.Add(prevTarget);
            }
        }

        private void InternalSingleTarget(Actor target)
        {
            var damage = new Damage(_damage, DamageType.Magical, DamageFlags.Ability);
            //TODO add targetable info
            // var targetable = target.GetComponent<Targetable>();
            // targetable.AffectSpell();
            var damageable = target.GetModule<IDamageable>();
            damageable.TakeDamage(target, damage);
            ApplyFX(target.transform);
        }

        private void ApplyFX(Transform target)
        {
            if (_unstableMagicFX == null)
                return;
            var instance = Instantiate(_unstableMagicFX, target.position, Quaternion.identity);
            if (!instance.TryGetComponent<FollowTarget>(out var follow))
                follow = instance.AddComponent<FollowTarget>();
            if (!instance.TryGetComponent<DieAfterParticleCompletion>(out var die))
                die = instance.AddComponent<DieAfterParticleCompletion>();
            if (instance.TryGetComponent<ParticleSystem>(out var ps))
                ps.Play();

            follow.SetTarget(target);
            die.AllowDeath();
        }

        private bool TrySearchTarget(Vector3 area, out Actor target) => TrySearchTarget(area, new Actor[0], out target);

        public AbilityPredicateBuilder Checker { get; set; }
        public SimpleAbilityView View { get; set; }
        public override IAbilityView GetAbilityView() => View;
        private bool TrySearchTarget(Vector3 area, ICollection<Actor> ignore, out Actor target)
        {
            var potentialTargets = Physics.OverlapSphere(area, _searchRange, (int) LayerMaskHelper.Entity);
            foreach (var potentialTarget in potentialTargets)
            {
                if (!AbilityHelper.TryGetActor(potentialTarget, out var actor))
                    continue;
                if (!Checker.AllowTarget(actor))
                    continue;
                if (ignore.Contains(actor))
                    continue;
 
                //TODO when targetable works, add this
                // if (!actor.TryGetComponent<ITargetable>(out _))
                // continue;
                if (!actor.TryGetModule<IDamageable>(out _))
                    continue;

                target = actor;
                return true;
            }

            target = default;
            return false;
        }


        public void Register(IStepableEvent source)
        {
            Checker.Cooldown.Register(source);
        }

        public void Unregister(IStepableEvent source)
        {
            Checker.Cooldown.Unregister(source);
        }

        public void CastReflected(Actor caster)
        {
            throw new System.NotImplementedException();
        }
    }
}