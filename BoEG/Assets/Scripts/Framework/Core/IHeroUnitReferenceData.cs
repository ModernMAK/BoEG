using Framework.Core.Modules;
using Framework.Types;

namespace Framework.Core
{
    public interface IHeroUnitReferenceData :
        IInstantiableData<IHealthableData>,
        IInstantiableData<IMovableData>,
        IInstantiableData<IArmorableData>,
        IInstantiableData<IAttackerableData>,
        IInstantiableData<IMagicableData>,
        IInstantiableData<IAggroableData>
    {
    }
}