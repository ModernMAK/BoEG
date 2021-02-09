using System;

namespace MobaGame.Framework.Core.Modules
{
    public class ModifiedArmor<TBlockMod,TResistMod> : IArmor, IInitializable<IArmorData>, IListener<IModifiable>, IArmorView
        where TBlockMod : IModifier 
        where TResistMod : IModifier
    {
        public ModifiedArmor(Func<TBlockMod,FloatModifier> getBlockMod, Func<TResistMod,FloatModifier> getResistMod)
        {
            Block = new ModifiedValueBoilerplate<TBlockMod>(getBlockMod);
            Resistance = new ModifiedValueBoilerplate<TResistMod>(getResistMod);
            Block.ModifierRecalculated += OnModifierChanged;
            Resistance.ModifierRecalculated += OnModifierChanged;
            Immunity = false;
        }

        private void OnModifierChanged(object sender, EventArgs e) => OnChanged();


        public ModifiedValueBoilerplate<TBlockMod> Block { get; }
        float IArmor.Block => Block.Value.Total;
        float IArmorView.Block => Block.Value.Total;

        public ModifiedValueBoilerplate<TResistMod> Resistance { get; }
        float IArmor.Resistance => Resistance.Value.Total;
        float IArmorView.Resistance => Resistance.Value.Total;

        public bool Immunity { get; private set; }

        public float CalculateReduction(float value) => ArmorX.CalculateReduction(value, Block.Value.Total, Resistance.Value.Total, Immunity);

        public void Initialize(IArmorData data)
        {
            Block.Value.Base = data.Block;
            Resistance.Value.Base = data.Resist;
            Immunity = data.Immune;
            OnChanged();
        }

        public void Register(IModifiable source)
        {
            Block.Register(source);
            Resistance.Register(source);
        }

        public void Unregister(IModifiable source)
        {
            Block.Unregister(source);
            Resistance.Unregister(source);
        }

        public event EventHandler Changed;
        protected void OnChanged() => Changed?.Invoke(this,EventArgs.Empty);
    }
}