namespace MobaGame.Framework.Core.Modules
{
	public interface IHealthCapacityModifier : IModifier
	{
        FloatModifier HealthCapacity { get; }
    }

}