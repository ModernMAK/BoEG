namespace MobaGame.Framework.Core.Modules
{
	public interface IHealthGenerationModifier : IModifier
	{
        Modifier HealthGeneration { get; }
	}

}