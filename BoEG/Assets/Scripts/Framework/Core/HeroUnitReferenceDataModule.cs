using Framework.Core.Modules;
using UnityEngine;

namespace Framework.Core
{
    public class HeroUnitReferenceDataModule : MonoBehaviour, IHeroUnitReferenceData
    {
        [SerializeField] private HeroUnitReferenceData referenceData;

        IAggroableData IInstantiableData<IAggroableData>.Data
        {
            get { return ((IInstantiableData<IAggroableData>) referenceData).Data; }
        }
        IHealthableData IInstantiableData<IHealthableData>.Data
        {
            get { return ((IInstantiableData<IHealthableData>) referenceData).Data; }
        }

        IMovableData IInstantiableData<IMovableData>.Data
        {
            get { return ((IInstantiableData<IMovableData>) referenceData).Data; }
        }

        IArmorableData IInstantiableData<IArmorableData>.Data
        {
            get { return ((IInstantiableData<IArmorableData>) referenceData).Data; }
        }

        IAttackerableData IInstantiableData<IAttackerableData>.Data
        {
            get { return ((IInstantiableData<IAttackerableData>) referenceData).Data; }
        }

        IMagicableData IInstantiableData<IMagicableData>.Data
        {
            get { return ((IInstantiableData<IMagicableData>) referenceData).Data; }
        }
    }
}