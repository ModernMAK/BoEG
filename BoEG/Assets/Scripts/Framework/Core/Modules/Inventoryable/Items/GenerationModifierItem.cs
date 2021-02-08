using System;
using UnityEngine;

namespace MobaGame.Framework.Core.Modules
{
    public class GenerationModifierItem : ModifierItem, IHealthGenerationModifier, IMagicGenerationModifier, IModifierView
    {
        public GenerationModifierItem(Sprite _icon, FloatModifier healthGen, FloatModifier manaGen)
        {
            HealthGeneration = healthGen;
            MagicGeneration = manaGen;
            Icon = _icon;
        }
        public FloatModifier HealthGeneration { get; }

        public FloatModifier MagicGeneration { get; }

        public override void Register(IModifiable source) => source.AddModifier(this);

        public override void Unregister(IModifiable source) => source.RemoveModifier(this);

        public IModifierView View => this;

        public event EventHandler Changed;

        public Sprite Icon { get; }
    
    }
}