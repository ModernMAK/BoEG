using Components.Buffable;
using Core;

namespace Components.Armorable
{
    public interface IArmorableInstance
    {
        float PhysicalBlock { get; }
        float PhysicalResist { get; }
        bool PhysicalImmunity { get; }
        float MagicalBlock { get; }
        float MagicalResist { get; }
        bool MagicalImmunity { get; }
        float CalculateReduction(float damage, DamageType type);
    }

    public interface IArmorableBuffInstance : IBuffInstance
    {
        float PhysicalBlockBonus { get; }
        float PhysicalResistkMultiplier { get; }
        bool ProvidePhysicalImmunity { get; }

        float MagicalBlockBonus { get; }
        float MagicalResistkMultiplier { get; }
        bool ProvideMagicalImmunity { get; }
    }
}