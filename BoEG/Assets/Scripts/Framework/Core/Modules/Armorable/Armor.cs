namespace MobaGame.Framework.Core.Modules
{
    public struct Armor
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
            if (value < Block)
                blocked += Resistance * (value - Block);
            if (blocked > value)
                blocked = value;
            return blocked;
        }
    }
}