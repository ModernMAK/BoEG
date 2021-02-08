namespace MobaGame.Framework.Core.Modules
{
    public interface IAttackDamageModifier : IModifier
    {
        FloatModifier AttackDamage { get; }
    }
}