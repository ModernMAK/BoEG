using MobaGame.Framework.Core.Modules;
using UnityEngine;

namespace MobaGame.Entity.UnitArchtypes
{
    public interface IBuildingData
    {
        HealthableData HealthableData { get; }
        ArmorableData ArmorableData { get; }
        AttackerableData AttackerableData { get; }
        Sprite Icon { get; }
    }
    [CreateAssetMenu(menuName = "Actor/Building/Generic")]
    public class BuildingData : ScriptableObject, IBuildingData
    {
#pragma warning disable 649
        [SerializeField] public HealthableData _healthableData = HealthableData.Default;
        [SerializeField] public ArmorableData _armorableData = ArmorableData.Default;
        [SerializeField] public AttackerableData _attackerableData = AttackerableData.Default;
        [SerializeField] public Sprite _icon;

        public HealthableData HealthableData => _healthableData;
        public ArmorableData ArmorableData => _armorableData;
        public AttackerableData AttackerableData => _attackerableData;
        public Sprite Icon => _icon;

#pragma warning restore 649
    }

    public interface ICoreData
    {
        HealthableData HealthableData { get; }
        ArmorableData ArmorableData { get; }
        Sprite Icon { get; }
        
    }
    [CreateAssetMenu(menuName = "Actor/Building/Core")]
    public class CoreData : ScriptableObject, ICoreData
    {
#pragma warning disable 649
        [SerializeField] public HealthableData _healthableData = HealthableData.Default;
        [SerializeField] public ArmorableData _armorableData = ArmorableData.Default;
        [SerializeField] public Sprite _icon;

        public HealthableData HealthableData => _healthableData;
        public ArmorableData ArmorableData => _armorableData;
        public Sprite Icon => _icon;

#pragma warning restore 649
    }
    
    public interface ITowerData
    {
        HealthableData HealthableData { get; }
        ArmorableData ArmorableData { get; }
        AttackerableData AttackerableData { get; }
        Sprite Icon { get; }
    }
    [CreateAssetMenu(menuName = "Actor/Building/Tower")]
    public class TowerData : ScriptableObject, ITowerData
    {
#pragma warning disable 649
        [SerializeField] public HealthableData _healthableData = HealthableData.Default;
        [SerializeField] public ArmorableData _armorableData = ArmorableData.Default;
        [SerializeField] public AttackerableData _attackerableData = AttackerableData.Default;
        [SerializeField] public Sprite _icon;

        public HealthableData HealthableData => _healthableData;
        public ArmorableData ArmorableData => _armorableData;
        public AttackerableData AttackerableData => _attackerableData;
        public Sprite Icon => _icon;

#pragma warning restore 649
    }
    
}