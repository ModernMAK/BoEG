namespace Components.Armorable
{
    public interface IArmorableData
    {
        float BasePhysicalBlock { get; }
        float GainPhysicalBlock { get; }
        float BasePhysicalResist { get; }
        float GainPhysicalResist { get; }
        bool BasePhysicalImmunity { get; }


        float BaseMagicalBlock { get; }
        float GainMagicalBlock { get; }
        float BaseMagicalResist { get; }
        float GainMagicalResist { get; }
        bool BaseMagicalImmunity { get; }
    }
}