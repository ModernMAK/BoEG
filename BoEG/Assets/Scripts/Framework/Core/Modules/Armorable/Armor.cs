namespace MobaGame.Framework.Core.Modules
{
    /// <summary>
    /// A structure which calculates damage block.
    /// </summary>
    public readonly struct Armor
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

        public float CalculateReduction(float value)
        {
            if (Immunity)
                return value;

            //First apply block
            var blocked = Block;

            //Avoids Block and resistance canceling out
            //If all damage is blocked, resistance (being applied after, has nothing to resist)
            if (value < Block)
                blocked += Resistance * (value - Block);
            //If resistance makes us block everything, return the value, otherwise only return blocked
            return blocked > value ? value : blocked;
        }
    }
}