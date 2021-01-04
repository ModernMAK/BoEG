namespace MobaGame.Framework.Core
{
    public struct ModifierResult
    {
        public float Flat { get; set; }
        public float Multiplier { get; set; }

        public float Calculate(float value)
        {
            return Flat + value * (1f + Multiplier);
        }
    }
}