namespace MobaGame.Framework.Core.Modules
{
    public interface IAttackRangeModifier : IModifier
    {
        FloatModifier AttackRange { get; }
    }
}