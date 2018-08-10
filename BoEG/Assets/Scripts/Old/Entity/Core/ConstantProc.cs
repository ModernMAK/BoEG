namespace Old.Entity.Core
{
    public class ConstantProc : RandomProc
    {
        public ConstantProc(float chance)
        {
            Chance = chance;
        }

        public float Chance { get; private set; }

        public override bool Proc()
        {
            return Random.NextDouble() <= Chance;
        }
    }
}