using System;
using System.Collections.Generic;
using MobaGame.Framework.Core;
using MobaGame.Framework.Core.Modules;
using MobaGame.Framework.Core.Modules.Ability;
using UnityEngine;

namespace MobaGame.Entity.UnitArchtypes
{
    [RequireComponent(typeof(HealthableModule))]
    [RequireComponent(typeof(MagicableModule))]
    [RequireComponent(typeof(ArmorableModule))]
    [RequireComponent(typeof(DamageTarget))]
    [RequireComponent(typeof(AttackerableModule))]
    [RequireComponent(typeof(Movable))]
    public class HeroModule : Actor
    {
        private Sprite _icon;
        public override Sprite GetIcon() => _icon;


        protected override void SetupComponents()
        {
            base.SetupComponents();
//            var buffable = new Buffable();
//            GetFrameworkComponent<IBuffable>().Initialize(buffable);

            _icon = Icon;
            if (TryGetInitializable<IHealthableData>(out var healthable))
                healthable.Initialize(HealthableData);
            else throw new MissingComponentException("IHealthable");


            if (TryGetInitializable<IMagicableData>(out var magicable))
                magicable.Initialize(MagicableData);
            else throw new MissingComponentException("IMagicable");


            if (TryGetInitializable<IArmorableData>(out var armorable))
                armorable.Initialize(ArmorableData);
            else throw new MissingComponentException("IArmorable");


            if (TryGetInitializable<IAttackerableData>(out var attackerable))
                attackerable.Initialize(AttackerableData);
            else throw new MissingComponentException("IAttackerable");

            if (TryGetInitializable<IMovableData>(out var movable))
                movable.Initialize(MovableData);
            else throw new MissingComponentException("IMovable");


            var instanceAbilities = new AbilityObject[AbilityData.Count];
            for (var i = 0; i < AbilityData.Count; i++) instanceAbilities[i] = Instantiate(AbilityData[i]);

            if (TryGetInitializable<IReadOnlyList<IAbility>>(out var abilitiable))
                abilitiable.Initialize(instanceAbilities);
            else throw new MissingComponentException("IAbilitiable");
        }
#pragma warning disable 649

        [SerializeField] private HeroData _data;

        protected IHealthableData HealthableData => _data._healthableData;
        protected IMagicableData MagicableData => _data._magicableData;
        protected IArmorableData ArmorableData => _data._armorableData;

        protected Sprite Icon => _data._icon;

        protected IAttackerableData AttackerableData => _data._attackerableData;
        protected IMovableData MovableData => _data._movableData;
        protected IReadOnlyList<AbilityObject> AbilityData => _data._abilities;
#pragma warning restore 649
    }
}