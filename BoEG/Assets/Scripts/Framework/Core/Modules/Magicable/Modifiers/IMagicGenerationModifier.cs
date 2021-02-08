namespace MobaGame.Framework.Core.Modules
{
	public interface IMagicGenerationModifier : IModifier
    {
        FloatModifier MagicGeneration { get; }
    }

}