
namespace MobaGame.Framework.Core.Modules
{
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