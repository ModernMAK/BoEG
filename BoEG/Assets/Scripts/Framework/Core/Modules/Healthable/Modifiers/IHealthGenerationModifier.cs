namespace MobaGame.Framework.Core.Modules
{
	public interface IHealthGenerationModifier : IModifier
	{
        FloatModifier HealthGeneration { get; }
	}

}