namespace MobaGame.Framework.Core.Modules
{
    public interface IPhysicalResistanceModifier : IModifier
    {
        public FloatModifier PhysicalResistance { get; }
    }
}