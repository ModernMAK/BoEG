using Components.Buffable;

namespace Components.Magicable
{
    public interface IMagicableBuffInstance : IBuffInstance    
    {
        float ManaCapacityBonus { get; }
        float ManaCapacityMultiplier { get; }
        float ManaGenBonus { get; }
        float ManaGenMultiplier { get; }
    }
}