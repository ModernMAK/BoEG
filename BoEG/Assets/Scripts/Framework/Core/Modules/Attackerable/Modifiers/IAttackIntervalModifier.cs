namespace MobaGame.Framework.Core.Modules
{
    public interface IAttackIntervalModifier : IModifier
    {
        FloatModifier AttackInterval { get; }
    }
}