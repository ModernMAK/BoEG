using Components.Buffable;

namespace Components.Attackerable
{
    public interface IAttackerableBuffInstance : IBuffInstance
    {
        float DamageBonus { get; }
        float DamageMultiplier { get; }
        
        float AttackRangeBonus { get; }
        
        float AttackSpeedBonus { get; }
    }
}