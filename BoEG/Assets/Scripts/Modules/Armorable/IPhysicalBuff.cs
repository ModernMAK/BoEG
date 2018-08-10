namespace Modules.Armorable
{
    public interface IPhysicalBuff
    {
        float PhysicalResistanceMultiplier { get; }
        float PhysicalBlockBonus { get; }
        float ProvidePhysicalImmunity { get; }
    }
}