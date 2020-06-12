namespace Framework.Core.Modules
{
    public struct ModifierResult
    {
        public float Flat { get; set; }
        public float Multiplier { get; set; }
        public float Calculate(float value) => Flat + value * (1f + Multiplier);
    }
}