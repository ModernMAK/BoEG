
using System;

namespace MobaGame.Framework.Core.Modules
{

    public class ModifiedArmor<TBlockMod,TResistMod> : IArmor, IInitializable<IArmorData>, IListener<IModifiable>
        where TBlockMod : IModifier 
        where TResistMod : IModifier
    {
        public ModifiedArmor(Func<TBlockMod,Modifier> getBlockMod, Func<TResistMod,Modifier> getResistMod)
        {
            Block = new ModifiedValueBoilerplate<TBlockMod>(getBlockMod);
            Resistance = new ModifiedValueBoilerplate<TResistMod>(getResistMod);
            Immunity = false;
		}


        public ModifiedValueBoilerplate<TBlockMod> Block { get; }
        float IArmor.Block => Block.Value.Total;

        public ModifiedValueBoilerplate<TResistMod> Resistance { get; }
        float IArmor.Resistance => Resistance.Value.Total;

        public bool Immunity { get; private set; }

        public float CalculateReduction(float value) => ArmorX.CalculateReduction(value, Block.Value.Total, Resistance.Value.Total, Immunity);

		public void Initialize(IArmorData data)
		{
            Block.Value.Base = data.Block;
            Resistance.Value.Base = data.Resist;
            Immunity = data.Immune;
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
	}
    public interface IArmorView
    {
        IModifiedValue<float> Block { get; }
        IModifiedValue<float> Resistance { get; }
        bool Immunity { get; }

    }
	public interface IArmor
	{
        float Block { get; }
        float Resistance { get; }
        bool Immunity { get; }
    }
    public static class ArmorX
	{

        public static float CalculateReduction(float value, float block=default, float resistance=default, bool immunity = false)
        {
            if (immunity)
                return value;

            //First apply block
            var blocked = block;

            //Avoids Block and resistance canceling out
            //If all damage is blocked, resistance (being applied after, has nothing to resist)
            if (value < block)
                blocked += resistance * (value - block);
            //If resistance makes us block everything, return the value, otherwise only return blocked
            return blocked > value ? value : blocked;
        }
    }

    /// <summary>
    /// A structure which calculates damage block.
    /// </summary>
    public readonly struct Armor : IArmor
    {
        
        public Armor(float block, float resistance, bool immunity)
        {
            Block = block;
            Resistance = resistance;
            Immunity = immunity;
        }

        public Armor(IArmorData data) : this(data.Block, data.Resist, data.Immune)
        {
        }

        public float Block { get; }
        public float Resistance { get; }
        public bool Immunity { get; }

        public float CalculateReduction(float value) => ArmorX.CalculateReduction(value, Block, Resistance, Immunity);
    }
}