using Old.Entity.Modules.Buffable;

namespace Old.Entity.Modules.Attackerable
{
    public interface IAttackerableBuffInstance : IBuffInstance
    {
        float DamageBonus { get; }
        float DamageMultiplier { get; }
        float AttackRangeBonus { get; }
        float AttackSpeedBonus { get; }
    }
}