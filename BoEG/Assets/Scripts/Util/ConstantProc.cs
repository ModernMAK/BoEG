namespace Util
{
    public class ConstantProc : RandomProc
    {
        public ConstantProc(float chance)
        {
            Chance = chance;
        }

        public float Chance { get; private set; }

        public void SetChace(float chance)
        {
            Chance = chance;
        }
        public override bool Proc()
        {
            return Random.NextDouble() <= Chance;
        }
    }
}