using System.Collections.Generic;
using Entity.Abilities.FlameWitch;
using Framework.Core.Modules;
using UnityEngine;

namespace Framework.Core
{
    [RequireComponent(typeof(Healthable))]
    [RequireComponent(typeof(Magicable))]
    [RequireComponent(typeof(Armorable))]
    [RequireComponent(typeof(DamageTarget))]
    [RequireComponent(typeof(Attackerable))]
    [RequireComponent(typeof(Movable))]
    public class Hero : Actor
    {
        protected override void SetupComponents()
        {
            base.SetupComponents();
//            var buffable = new Buffable();
//            GetFrameworkComponent<IBuffable>().Initialize(buffable);

            if (TryGetInitializable<IHealthableData>(out var healthable))
                healthable.Initialize(HealthableData);
            else throw new MissingComponentException("IHealthable");


            if (TryGetInitializable<IMagicableData>(out var magicable))
                magicable.Initialize(MagicableData);
            else throw new MissingComponentException("IMagicable");


            if (TryGetInitializable<IArmorableData>(out var armorable))
                armorable.Initialize(ArmorableData);
            else throw new MissingComponentException("IArmorable");

//            GetFrameworkComponent<IDamageTarget>().Initialize();


            // GetInitializable<IAttackerableData>().Initialize(_attackerableData);

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

        protected IAttackerableData AttackerableData => _data._attackerableData;
        protected IMovableData MovableData => _data._movableData;
        protected IReadOnlyList<AbilityObject> AbilityData => _data._abilities;
#pragma warning restore 649
    }
}