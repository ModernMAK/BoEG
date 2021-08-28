namespace MobaGame.Framework.Core.Modules
{
    public interface IPhysicalBlockModifier : IModifier
    {
        public FloatModifier PhysicalBlock { get; }
    }
}