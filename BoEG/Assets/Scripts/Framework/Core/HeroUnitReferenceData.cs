using System;
using Framework.Core.Modules;
using Framework.Types;
using UnityEngine;
using UnityEngine.Serialization;

namespace Framework.Core
{
    [Serializable]
    [CreateAssetMenu(menuName = "Unit/Hero")]
    public class HeroUnitReferenceData : ScriptableObject, IHeroUnitReferenceData
    {
        [SerializeField] private HealthableData _healthable;
        [SerializeField] private MovableData _movable;
        [SerializeField] private ArmorableData _armorable;
        [SerializeField] private AttackerableData _attackerable;
        [SerializeField] private MagicableData _magicable;


        IHealthableData IInstantiableData<IHealthableData>.Data
        {
            get { return _healthable; }
        }

        IMovableData IInstantiableData<IMovableData>.Data
        {
            get { return _movable; }
        }

        IArmorableData IInstantiableData<IArmorableData>.Data
        {
            get { return _armorable; }
        }

        IAttackerableData IInstantiableData<IAttackerableData>.Data
        {
            get { return _attackerable; }
        }

        public IMagicableData Data
        {
            get { return _magicable; }
        }
    }
}