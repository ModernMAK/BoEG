using MobaGame.Framework.Core.Modules;
using UnityEngine;

namespace MobaGame.Entity.UnitArchtypes
{
    [CreateAssetMenu(menuName = "Actor/Unit")]
    public class UnitData : ScriptableObject, IUnitData
    {
#pragma warning disable 649
        [SerializeField] public AggroableData _aggroableData = AggroableData.Default;
        [SerializeField] public HealthableData _healthableData = HealthableData.Default;
        [SerializeField] public MagicableData _magicableData = MagicableData.Default;
        [SerializeField] public ArmorableData _armorableData = ArmorableData.Default;
        [SerializeField] public AttackerableData _attackerableData = AttackerableData.Default;
        [SerializeField] public MovableData _movableData = MovableData.Default;
        [SerializeField] public Sprite _icon;

        public AggroableData AggroableData => _aggroableData;
        public HealthableData HealthableData => _healthableData;
        public MagicableData MagicableData => _magicableData;
        public ArmorableData ArmorableData => _armorableData;
        public AttackerableData AttackerableData => _attackerableData;
        public MovableData MovableData => _movableData;
        public Sprite Icon => _icon;
#pragma warning restore 649
    }
}