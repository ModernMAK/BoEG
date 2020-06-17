using System.Collections.Generic;
using Entity.Abilities.FlameWitch;
using Framework.Core.Modules;
using UnityEngine;

namespace Framework.Core
{
    [RequireComponent(typeof(Healthable))]
    [RequireComponent(typeof(Armorable))]
    [RequireComponent(typeof(DamageTarget))]
    [RequireComponent(typeof(Attackerable))]
    public class Building : Actor
    {
        private Sprite _icon;
        public override Sprite GetIcon() => _icon;

        protected override void SetupComponents()
        {
            base.SetupComponents();

            _icon = Icon;
            if (TryGetInitializable<IHealthableData>(out var healthable))
                healthable.Initialize(HealthableData);
            else throw new MissingComponentException("IHealthable");


            if (TryGetInitializable<IArmorableData>(out var armorable))
                armorable.Initialize(ArmorableData);
            else throw new MissingComponentException("IArmorable");


            if (TryGetInitializable<IAttackerableData>(out var attackerable))
                attackerable.Initialize(AttackerableData);
            else throw new MissingComponentException("IAttackerable");
        }
#pragma warning disable 649

        [SerializeField] private BuildingData _data;

        protected IHealthableData HealthableData => _data._healthableData;
        protected IArmorableData ArmorableData => _data._armorableData;

        protected Sprite Icon => _data._icon;

        protected IAttackerableData AttackerableData => _data._attackerableData;
#pragma warning restore 649
    }
}