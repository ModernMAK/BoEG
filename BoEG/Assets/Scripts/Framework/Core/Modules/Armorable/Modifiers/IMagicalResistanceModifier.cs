namespace MobaGame.Framework.Core.Modules
{
    public interface IMagicalResistanceModifier : IModifier
    {
        public FloatModifier MagicalResistance { get; }
    }
}