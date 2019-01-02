using Framework.Core.Modules;
using Framework.Types;
using UnityEngine;
using UnityEngine.Serialization;

namespace Framework.Core
{
    
    //Creep
    //[ ] Armorable
    //[x] Attackerable
    //[/] Commandable
    //    -> 'Director' Issues Commands 
    //[x] Damagable
    //[x] Healthable
    //[/] Magicable
    //    -> Not All Creeps Are Magicable
    //[x] Targetable
    
    
    public class HeroUnit : Unit, IHeroUnitReferenceData
    {
        [SerializeField] private HeroUnitReferenceData referenceData;

        protected override void StartLogic()
        {
            base.StartLogic();
            Instanitate();
            Spawn();
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