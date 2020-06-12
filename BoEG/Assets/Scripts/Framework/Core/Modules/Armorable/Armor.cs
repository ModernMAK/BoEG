namespace Framework.Core.Modules
{
    public struct Armor
    {
        public Armor(float block, float resistance, bool immunity)
        {
            Block = block;
            Resistance = resistance;
            Immunity = immunity;
        }
        public Armor(IArmorData data) : this(data.Block,data.Resist,data.Immune)
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
            var result = Resistance * (value - Block);
            var blocked = value - result;
            //Only happens if Resistance > 1
            if (blocked > value)
                blocked = value;
            return blocked;
        }
    }
}