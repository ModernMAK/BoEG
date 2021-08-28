namespace MobaGame.Framework.Core.Modules
{
    public interface IMagicalBlockModifier : IModifier
    {
        public FloatModifier MagicalBlock { get; }
    }
}