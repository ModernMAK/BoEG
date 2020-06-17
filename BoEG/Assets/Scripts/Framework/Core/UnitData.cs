using Framework.Core.Modules;
using UnityEngine;

namespace Framework.Core
{
    [CreateAssetMenu(menuName = "Actor/Unit")]
    public class UnitData : ScriptableObject
    {
#pragma warning disable 649
        [SerializeField] public HealthableData _healthableData = HealthableData.Default;

        [SerializeField] public MagicableData _magicableData = MagicableData.Default;
        [SerializeField] public ArmorableData _armorableData = ArmorableData.Default;
        [SerializeField] public AttackerableData _attackerableData = AttackerableData.Default;
        [SerializeField] public MovableData _movableData = MovableData.Default;
        [SerializeField] public Sprite _icon;
#pragma warning restore 649
    }
}