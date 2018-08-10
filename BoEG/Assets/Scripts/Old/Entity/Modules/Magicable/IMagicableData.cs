namespace Old.Entity.Modules.Magicable
{
    public interface IMagicableData
    {
        float BaseManaCapacity { get; }
        float BaseManaGen { get; }
        float GainManaCapacity { get; }
        float GainManaGen { get; }
    }
}