using System;
using Modules.Abilityable;
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
    public class HeroData : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private Material _mat;

        [SerializeField] private AbilityableData _abilityable;
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

        public IHealthableData Healthable
        {
            get { return _healthable; }
        }

        public IArmorableData Armorable
        {
            get { return _armorable; }
        }

        public IAttackerableData Attackerable
        {
            get { return _attackerable; }
        }

        public IAbilityableData Abilityable
        {
            get { return _abilityable; }
        }

        public IMovableData Movable
        {
            get { return _movable; }
        }

        public IMagicableData Magicable
        {
            get { return _magicable; }
        }
    }
}