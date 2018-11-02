using System;
using System.Collections.Generic;
using Core;
using Modules.Abilityable;
using Modules.Abilityable.Ability;
using Modules.Armorable;
using Modules.Attackerable;
using Modules.Healthable;
using Modules.Magicable;
using Modules.Movable;
using UnityEngine;

namespace Entity
{
    [Serializable]
    [CreateAssetMenu(fileName = "Hero.asset", menuName = "Entity/Hero")]
    public class HeroData : ScriptableObject, IEntityData,
        IAbilitiableData, IArmorableData, IAttackerableData,
        IHealthableData, IMagicableData, IMovableData
    {
        [SerializeField] private string _name;
        [SerializeField] private Material _mat;
        [SerializeField] private Sprite _icon;

        [SerializeField] private AbilitiableData _abilitiable;
        [SerializeField] private ArmorableData _armorable = ArmorableData.Default;
        [SerializeField] private AttackerableData _attackerable = AttackerableData.Default;
        [SerializeField] private HealthableData _healthable = HealthableData.Default;
        [SerializeField] private MagicableData _magicable = MagicableData.Default;
        [SerializeField] private MovableData _movable = MovableData.Default;

        public string Name
        {
            get { return _name; }
        }

        public Material Mat
        {
            get { return _mat; }
        }

        public Sprite Icon
        {
            get { return _icon; }
        }

        [Obsolete("Use interface directly")]
        public IHealthableData Healthable
        {
            get { return _healthable; }
        }

        [Obsolete("Use interface directly")]
        public IArmorableData Armorable
        {
            get { return _armorable; }
        }

        [Obsolete("Use interface directly")]
        public IAttackerableData Attackerable
        {
            get { return _attackerable; }
        }

        [Obsolete("Use interface directly")]
        public IAbilitiableData Abilitiable
        {
            get { return _abilitiable; }
        }

        [Obsolete("Use interface directly")]
        public IMovableData Movable
        {
            get { return _movable; }
        }

        [Obsolete("Use interface directly")]
        public IMagicableData Magicable
        {
            get { return _magicable; }
        }

        #region Abilitiable

        public IEnumerable<IAbilityData> Abilities
        {
            get { return _abilitiable.Abilities; }
        }

        public int AbilityCount
        {
            get { return _abilitiable.AbilityCount; }
        }

        #endregion

        #region Armorable

        public ArmorData Physical
        {
            get { return _armorable.Physical; }
        }

        public ArmorData Magical
        {
            get { return _armorable.Magical; }
        }

        #endregion

        #region Attackerable

        public FloatScalar AttackDamage
        {
            get { return _attackerable.AttackDamage; }
        }

        public FloatScalar AttackSpeed
        {
            get { return _attackerable.AttackSpeed; }
        }

        public float AttackRange
        {
            get { return _attackerable.AttackRange; }
        }

        public GameObject Projectile
        {
            get { return _attackerable.Projectile; }
        }

        #endregion

        #region Healthable

        public FloatScalar HealthCapacity
        {
            get { return _healthable.HealthCapacity; }
        }

        public FloatScalar HealthGeneration
        {
            get { return _healthable.HealthGeneration; }
        }

        #endregion

        #region Magicable

        public FloatScalar ManaCapacity
        {
            get { return _magicable.ManaCapacity; }
        }

        public FloatScalar ManaGeneration
        {
            get { return _magicable.ManaGeneration; }
        }

        #endregion

        #region Movable

        public float MoveSpeed
        {
            get { return _movable.MoveSpeed; }
        }

        public float TurnSpeed
        {
            get { return _movable.TurnSpeed; }
        }

        #endregion
    }
}