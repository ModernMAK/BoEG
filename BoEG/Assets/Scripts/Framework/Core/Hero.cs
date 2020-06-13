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
        protected override void Awake()
        {
            base.Awake();
        }

        protected override void SetupComponents()
        {
            base.SetupComponents();
//            var buffable = new Buffable();
//            GetFrameworkComponent<IBuffable>().Initialize(buffable);

            if (TryGetInitializable<IHealthableData>(out var healthable))
                healthable.Initialize(_healthableData);
            else throw new MissingComponentException("IHealthable");


            if (TryGetInitializable<IMagicableData>(out var magicable))
                magicable.Initialize(_magicableData);
            else throw new MissingComponentException("IMagicable");


            if (TryGetInitializable<IArmorableData>(out var armorable))
                armorable.Initialize(_armorableData);
            else throw new MissingComponentException("IArmorable");

//            GetFrameworkComponent<IDamageTarget>().Initialize();


            // GetInitializable<IAttackerableData>().Initialize(_attackerableData);

            if (TryGetInitializable<IMovableData>(out var movable))
                movable.Initialize(_movableData);
            else throw new MissingComponentException("IMovable");


            for (var i = 0; i < _abilities.Length; i++) _abilities[i] = Instantiate(_abilities[i]);

            if (TryGetInitializable<IReadOnlyList<IAbility>>(out var abilitiable))
                abilitiable.Initialize(_abilities);
            else throw new MissingComponentException("IAbilitiable");
        }

#pragma warning disable 649
        [SerializeField] private HealthableData _healthableData;
        [SerializeField] private MagicableData _magicableData;
        [SerializeField] private ArmorableData _armorableData;
        [SerializeField] private AttackerableData _attackerableData;
        [SerializeField] private MovableData _movableData;
        [SerializeField] private AbilityObject[] _abilities;
#pragma warning restore 649
    }
}