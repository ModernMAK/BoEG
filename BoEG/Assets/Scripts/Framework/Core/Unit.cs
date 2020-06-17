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
    public class Unit : Actor
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

//            GetFrameworkComponent<IDamageTarget>().Initialize();


            if (TryGetInitializable<IAttackerableData>(out var attackerable))
                attackerable.Initialize(AttackerableData);
            else throw new MissingComponentException("IMovable");

            if (TryGetInitializable<IMovableData>(out var movable))
                movable.Initialize(MovableData);
            else throw new MissingComponentException("IMovable");
        }
#pragma warning disable 649

        [SerializeField] private UnitData _data;

        protected IHealthableData HealthableData => _data._healthableData;
        protected IMagicableData MagicableData => _data._magicableData;
        protected IArmorableData ArmorableData => _data._armorableData;

        protected Sprite Icon => _data._icon;

        protected IAttackerableData AttackerableData => _data._attackerableData;
        protected IMovableData MovableData => _data._movableData;
#pragma warning restore 649
    }
}