using Framework.Core.Modules;

namespace Framework.Core
{
    public interface IHeroUnitReferenceData :
        IInstantiableData<IHealthableData>,
        IInstantiableData<IMovableData>,
        IInstantiableData<IArmorableData>,
        IInstantiableData<IAttackerableData>,
        IInstantiableData<IMagicableData>
    {
    }
}