using Old.Entity.Modules.Buffable;

namespace Old.Entity.Modules.Magicable
{
    public interface IMagicableBuffInstance : IBuffInstance    
    {
        float ManaCapacityBonus { get; }
        float ManaCapacityMultiplier { get; }
        float ManaGenBonus { get; }
        float ManaGenMultiplier { get; }
    }
}