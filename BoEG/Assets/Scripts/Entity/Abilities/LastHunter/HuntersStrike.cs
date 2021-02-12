using MobaGame.Framework.Core;
using MobaGame.Framework.Core.Modules;
using MobaGame.Framework.Core.Modules.Ability;
using MobaGame.Framework.Core.Modules.Ability.Helpers;
using MobaGame.Framework.Types;
using UnityEngine;

namespace MobaGame.Entity.Abilities.LastHunter
{
    [CreateAssetMenu(menuName = "Ability/LastHunter/HunterStrike")]
    public class HuntersStrike : AbilityObject, IReflectableAbility, IListener<IStepableEvent>
    {
        /* Ground target
         * Blink To target
         * Deal damage to units between origin and target
         */
#pragma warning disable 0649
        [SerializeField] private Sprite _icon;
        private const float _pathWidth = 1f;
        [SerializeField] private float _bonusDamage;
        [SerializeField] private float _castRange;
        [SerializeField] private float _manaCost;
        [SerializeField] private float _cooldown;
        private CastRange CastRange;
#pragma warning restore 0649

        public override void Initialize(Actor data)
        {
            base.Initialize(data);
            CastRange = new CastRange(data.transform){MaxDistance = _castRange};
            Checker = new AbilityPredicateBuilder(data)
            {
                AllowSelf = false,
                Cooldown = new Cooldown(_cooldown),
                MagicCost = new MagicCost(Modules.Magicable,_manaCost),
                Teamable = TeamableChecker.NonAllyOnly(Modules.Teamable),

            };
            View = new SimpleAbilityView()
            {
                StatCost = Checker.MagicCost,
                Cooldown = Checker.Cooldown,
                Icon = _icon,
            };
            Checker.RebuildChecks();
            Register(data);
        }

        public AbilityPredicateBuilder Checker { get; set; }
        public SimpleAbilityView View { get; set; }
        public override IAbilityView GetAbilityView() => View;
        
        public override void ConfirmCast()
        {
            if (!Checker.AllowCast())
                return;

            var ray = AbilityHelper.GetScreenRay();
            if (!AbilityHelper.TryGetWorldOrEntity(ray, out var hit))
                return;
            var unitCast = AbilityHelper.TryGetActor(hit.collider, out var actor);
            if(actor == Self)
                return;
            var position = unitCast
                ? actor.transform.position
                : hit.point;
            if (!CastRange.InRange(position))
                return;

            if (unitCast)
            {
                if (!Checker.AllowTarget(actor))
                    return;
                
                Checker.DoCast();
                CastObjectTarget(actor);
            }
            else
            {
                Checker.DoCast();
                CastGroundTarget(position);
            }

            Modules.Abilitiable.NotifyAbilityCast(new AbilityEventArgs(Self, Checker.MagicCost.Cost));
        }

        public void CastGroundTarget(Vector3 worldPos)
        {
            var origin = Self.transform.position;
            var length = AbilityHelper.GetLineLength(origin, worldPos);
            var boxHalfExtents = new Vector3(_pathWidth, 2f, length);
            var rotation = AbilityHelper.GetRotation(origin, worldPos);
            var center = AbilityHelper.GetBoxCenter(origin, boxHalfExtents, rotation);

            var colliders = Physics.OverlapBox(center, boxHalfExtents, rotation, (int) LayerMaskHelper.Entity);

            var atkDamage = Modules.Attackerable.Damage;

            var dmg = new Damage(_bonusDamage + atkDamage, DamageType.Physical,
                DamageFlags.Ability | DamageFlags.Attack);

            foreach (var col in colliders)
            {
                if (!AbilityHelper.TryGetActor(col, out var actor))
                    continue;
                if(!Checker.AllowTarget(actor))
                    continue;
                Modules.Attackerable.RawAttack(actor, dmg);
            }

            Modules.Movable.WarpTo(worldPos);
            Modules.Commandable.ClearCommands();
        }

        //We jump behind them
        public void CastObjectTarget(Actor target)
        {
            var origin = Self.transform.position;
            var dest = target.transform.position;
            var offset = dest - origin;
            const float JumpDistance = 0.5f;//As a benefit; it's range is extended slightly
            CastGroundTarget(dest + offset.normalized * JumpDistance);
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