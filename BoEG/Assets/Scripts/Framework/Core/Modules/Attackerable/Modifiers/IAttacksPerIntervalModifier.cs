namespace MobaGame.Framework.Core.Modules
{
    public interface IAttacksPerIntervalModifier : IModifier
    {
        FloatModifier AttacksPerInterval { get; }
    }
}