namespace Modules.Armorable
{
    public interface IMagicalBuff
    {
        float MagicalResistanceMultiplier { get; }
        float MagicalBlockBonus { get; }
        float ProvideMagicalImmunity { get; }
    }
}