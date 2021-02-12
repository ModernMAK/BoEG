using MobaGame.Framework.Core;
using MobaGame.Framework.Core.Modules;
using MobaGame.Framework.Core.Modules.Ability;
using MobaGame.Framework.Core.Modules.Ability.Helpers;
using MobaGame.Framework.Types;
using UnityEngine;

namespace MobaGame.Entity.Abilities.FlameWitch
{
    [CreateAssetMenu(menuName = "Ability/FlameWitch/FireBall")]
    public class Fireball : AbilityObject, IListener<IStepableEvent>
    {
#pragma warning disable 0649
        [SerializeField] private Sprite _icon;
        private Overheat _overheat;
        [Header("Mana")] [SerializeField] private float _manaCost;
        [Header("Damage")] [SerializeField] private float _damage;

        [Header("Cooldown")] [SerializeField] private float _cooldown;
        private DurationTimer _cooldownTimer;

        [Header("Cast Range")] [SerializeField]
        private float _castRange;

        [SerializeField] private float _overheatCastRange;
        [SerializeField] private float _pathWidth;
#pragma warning restore 0649
      
        private AbilityPredicateBuilder CheckBuilder { get; set; }
        private SimpleAbilityView View { get; set; }
        public override IAbilityView GetAbilityView() => View;

        private CastRange CastRange { get; set; }
        private float CurrentCastRange => _overheat.Active ? _overheatCastRange : _castRange;

        /* Ground-Target Spell
         * Deals damage along path.
         * When OverHeating;
         *     Path is longer.
         */
        public override void Initialize(Actor data)
        {
            base.Initialize(data);
            _cooldownTimer = new DurationTimer(_cooldown,true);
            Modules.Abilitiable.TryGetAbility(out _overheat);
            CastRange = new CastRange(data.transform){MaxDistance = _castRange};//Not part of targeting checks
            CheckBuilder = new AbilityPredicateBuilder(data)
            {
                Teamable = TeamableChecker.NonAllyOnly(Modules.Teamable),
                MagicCost = new MagicCost(Modules.Magicable,_manaCost),
                Cooldown = new Cooldown(_cooldown),
                AllowSelf = false,
            };
            View = new SimpleAbilityView()
            {
                Icon = _icon,
                Cooldown = CheckBuilder.Cooldown,
                StatCost = CheckBuilder.MagicCost,
            };
            CheckBuilder.RebuildChecks();
            Register(data);
        }

        public override void ConfirmCast()
        {
            var ray = AbilityHelper.GetScreenRay();
            if (!AbilityHelper.TryGetWorld(ray, out var hit))
                return;
            if(!CheckBuilder.AllowCast())
                return;
            CastRange.MaxDistance = _overheat.Active ? _overheatCastRange : _castRange;
            if(!CastRange.InRange(hit.point))
                return;
            CheckBuilder.DoCast();
            CastGroundTarget(hit.point);
            Modules.Abilitiable.NotifyAbilityCast(new AbilityEventArgs(Self, View.StatCost.Cost));
        }

        public void CastGroundTarget(Vector3 worldPos)
        {
            var origin = Self.transform.position;
            var boxBaseSize = new Vector2(_pathWidth, 2);
            var length = AbilityHelper.GetLineLength(origin, worldPos);
            length = Mathf.Min(length, CurrentCastRange);
            var boxHalfExtents = new Vector3(boxBaseSize.x, boxBaseSize.y, length) / 2;
            var rotation = AbilityHelper.GetRotation(origin, worldPos);
            var center = AbilityHelper.GetBoxCenter(origin, boxHalfExtents, rotation);
            var colliders = Physics.OverlapBox(center, boxHalfExtents, rotation, (int) LayerMaskHelper.Entity);
            var dmg = new Damage(_damage, DamageType.Magical, DamageFlags.Ability);

            foreach (var col in colliders)
            {
                if (!AbilityHelper.TryGetActor(col, out var actor))
                    continue; //Not an actor, ignore

                if (!CheckBuilder.AllowTarget(actor))
                    continue; //Ignore if allies

                if (!actor.TryGetModule<IDamageable>(out var damageTarget))
                    continue; //Ignore if cant damage

                damageTarget.TakeDamage(Self, dmg);
            }
        }

        public void Register(IStepableEvent source)
        {
            CheckBuilder.Cooldown.Register(source);
        }


        public void Unregister(IStepableEvent source)
        {
            CheckBuilder.Cooldown.Unregister(source);
        }
    }
}