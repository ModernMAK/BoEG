using System.Collections.Generic;
using Components.Abilityable;
using Components.Armorable;
using Components.Attackerable;
using Components.Healthable;
using Components.Levelable;
using Components.Magicable;
using Components.Movable;
using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "Hero.asset", menuName = "Entity/Hero")]
    public class HeroData : NamedEntityData, IAbilityableData, IHealthableData, IMagicableData, IMovableData,
        IArmorableData, IAttackerableData, ILevelableData
    {
        [SerializeField] private AbilityableData _abilityableData;
        [SerializeField] private ArmorableData _armorableData;
        [SerializeField] private AttackerableData _attackerableData;
        [SerializeField] private HealthableData _healthableData;
        [SerializeField] private LevelableData _levelableData;
        [SerializeField] private MagicableData _magicableData;
        [SerializeField] private MovableData _movableData;

        public IEnumerable<IAbilityData> Abilities
        {
            get { return _abilityableData.Abilities; }
        }

        public int AbilityCount
        {
            get { return _abilityableData.AbilityCount; }
        }

        public float BaseHealthCapacity
        {
            get { return _healthableData.BaseHealthCapacity; }
        }

        public float BaseHealthGen
        {
            get { return _healthableData.BaseHealthGen; }
        }

        public float GainHealthCapacity
        {
            get { return _healthableData.GainHealthCapacity; }
        }

        public float GainHealthGen
        {
            get { return _healthableData.GainHealthGen; }
        }

        public float BaseManaCapacity
        {
            get { return _magicableData.BaseManaCapacity; }
        }

        public float BaseManaGen
        {
            get { return _magicableData.BaseManaGen; }
        }

        public float GainManaCapacity
        {
            get { return _magicableData.GainManaCapacity; }
        }

        public float GainManaGen
        {
            get { return _magicableData.GainManaGen; }
        }

        public float BaseMoveSpeed
        {
            get { return _movableData.BaseMoveSpeed; }
        }

        public float BaseTurnSpeed
        {
            get { return _movableData.BaseTurnSpeed; }
        }

        public float BasePhysicalBlock
        {
            get { return _armorableData.BasePhysicalBlock; }
        }

        public float GainPhysicalBlock
        {
            get { return _armorableData.GainPhysicalBlock; }
        }

        public float BasePhysicalResist
        {
            get { return _armorableData.BasePhysicalResist; }
        }

        public float GainPhysicalResist
        {
            get { return _armorableData.GainPhysicalResist; }
        }

        public bool HasPhysicalImmunity
        {
            get { return _armorableData.HasPhysicalImmunity; }
        }

        public float BaseMagicalBlock
        {
            get { return _armorableData.BaseMagicalBlock; }
        }

        public float GainMagicalBlock
        {
            get { return _armorableData.GainMagicalBlock; }
        }

        public float BaseMagicalResist
        {
            get { return _armorableData.BaseMagicalResist; }
        }

        public float GainMagicalResist
        {
            get { return _armorableData.GainMagicalResist; }
        }

        public bool HasMagicalImmunity
        {
            get { return _armorableData.HasMagicalImmunity; }
        }

        public float BaseDamage
        {
            get { return _attackerableData.BaseDamage; }
        }

        public float BaseAttackRange
        {
            get { return _attackerableData.BaseAttackRange; }
        }

        public float BaseAttackSpeed
        {
            get { return _attackerableData.BaseAttackSpeed; }
        }

        public float GainDamage
        {
            get { return _attackerableData.GainDamage; }
        }

        public float GainAttackRange
        {
            get { return _attackerableData.GainAttackRange; }
        }

        public float GainAttackSpeed
        {
            get { return _attackerableData.GainAttackSpeed; }
        }

        public int InitialLevel
        {
            get { return _levelableData.InitialLevel; }
        }

        public int MaxLevel
        {
            get { return _levelableData.MaxLevel; }
        }

        public int[] ExperienceCurve
        {
            get { return _levelableData.ExperienceCurve; }
        }
    }
}

