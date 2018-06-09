namespace Components.Magicable
{
    public interface IMagicable 
    {
        float ManaPoints { get; set; }
        float ManaRatio { get; set; }
        float ManaCapacity { get; }
        float ManaGen { get; }
    }
}