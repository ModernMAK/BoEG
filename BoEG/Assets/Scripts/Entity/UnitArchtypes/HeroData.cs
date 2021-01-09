using System.Collections.Generic;
using MobaGame.Framework.Core.Modules;
using MobaGame.Framework.Core.Modules.Ability;
using UnityEngine;

namespace MobaGame.Entity.UnitArchtypes
{
    [CreateAssetMenu(menuName = "Actor/Hero")]
    public class HeroData : ScriptableObject, IHeroData
    {
#pragma warning disable 649
        [SerializeField] public HealthableData _healthableData;

        [SerializeField] public MagicableData _magicableData;
        [SerializeField] public ArmorableData _armorableData;
        [SerializeField] public AttackerableData _attackerableData;
        [SerializeField] public MovableData _movableData;
        [SerializeField] public AbilityObject[] _abilities;
        [SerializeField] public Sprite _icon;
#pragma warning restore 649
        public Sprite Icon => _icon;
        public IHealthableData HealthableData => _healthableData;
        public IMagicableData MagicableData => _magicableData;
        public IArmorableData ArmorableData => _armorableData;
        public IAttackerableData AttackerableData => _attackerableData;
        public IMovableData MovableData => _movableData;
        public IReadOnlyList<AbilityObject> Abilities => _abilities;
    }

    public interface IHeroData
    {
        public Sprite Icon { get; }
        public IHealthableData HealthableData { get; }
        public IMagicableData MagicableData { get; }
        public IArmorableData ArmorableData { get; }

        public IAttackerableData AttackerableData { get; }
        public IMovableData MovableData { get; }
        public IReadOnlyList<AbilityObject> Abilities { get; }
    }
}