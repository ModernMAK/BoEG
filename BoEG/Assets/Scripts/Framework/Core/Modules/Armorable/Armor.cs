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
    }
}