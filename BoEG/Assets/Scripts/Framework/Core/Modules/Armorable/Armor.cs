
namespace MobaGame.Framework.Core.Modules
{

    public class NewArmor : IArmor, IInitializable<IArmorData>
    {
        public NewArmor()
		{
            Block = new ModifiedValue();
            Resistance = new ModifiedValue();
            Immunity = false;
		}
        public NewArmor(float block, float resistance, bool immunity)
        {
            Block = new ModifiedValue(block);
            Resistance = new ModifiedValue(resistance);
            Immunity = immunity;
        }

        public NewArmor(IArmorData data) : this(data.Block, data.Resist, data.Immune)
        {

        }

        public ModifiedValue Block { get; }
        IModifiedValue<float> IArmor.Block => Block;

        public ModifiedValue Resistance { get; }
        IModifiedValue<float> IArmor.Resistance => Block;

        public bool Immunity { get; set; }

        public float CalculateReduction(float value) => ArmorX.CalculateReduction(value, Block.Total, Resistance.Total, Immunity);

		public void Initialize(IArmorData data)
		{
            Block.Base = data.Block;
            Resistance.Base = data.Resist;
            Immunity = data.Immune;
		}
	}
	public interface IArmor
	{
        IModifiedValue<float> Block { get; }
        IModifiedValue<float> Resistance { get; }
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

        IModifiedValue<float> IArmor.Block => new ModifiedValue(Block);

		IModifiedValue<float> IArmor.Resistance => new ModifiedValue(Resistance);

        public float CalculateReduction(float value) => ArmorX.CalculateReduction(value, Block, Resistance, Immunity);
    }
}