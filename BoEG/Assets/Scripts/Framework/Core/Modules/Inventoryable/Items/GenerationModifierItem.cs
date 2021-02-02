namespace MobaGame.Framework.Core.Modules
{
    public class GenerationModifierItem : ModifierItem, IHealthGenerationModifier, IMagicGenerationModifier
    {
        public GenerationModifierItem(FloatModifier healthGen, FloatModifier manaGen)
        {
            HealthGeneration = healthGen;
            MagicGeneration = manaGen;
        }
        public FloatModifier HealthGeneration { get; }

        public FloatModifier MagicGeneration { get; }

        public override void Register(IModifiable source) => source.AddModifier(this);

        public override void Unregister(IModifiable source) => source.RemoveModifier(this);
    }
}