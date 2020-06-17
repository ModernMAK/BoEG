using Framework.Core.Modules;
using UnityEngine;

namespace Framework.Core
{
    [CreateAssetMenu(menuName = "Actor/Building")]
    public class BuildingData : ScriptableObject
    {
#pragma warning disable 649
        [SerializeField] public HealthableData _healthableData = HealthableData.Default;
        [SerializeField] public ArmorableData _armorableData = ArmorableData.Default;
        [SerializeField] public AttackerableData _attackerableData = AttackerableData.Default;
        [SerializeField] public Sprite _icon;
#pragma warning restore 649
    }
}