namespace Components.Magicable
{
    public interface IMagicableData
    {
        float BaseManaCapacity { get; }
        float BaseManaGen { get; }
        float GainManaCapacity { get; }
        float GainManaGen { get; }
    }
}