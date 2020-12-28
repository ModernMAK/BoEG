using Entity.Abilities.FlameWitch;
using Framework.Ability;
using Framework.Core.Modules;
using UnityEngine;

namespace Framework.Core
{
    [CreateAssetMenu(menuName = "Actor/Hero")]
    public class HeroData : ScriptableObject
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
    }
}