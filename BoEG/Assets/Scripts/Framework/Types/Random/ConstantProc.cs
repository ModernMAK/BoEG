namespace Framework.Types
{
    public class ConstantProc : RandomProc
    {
        public ConstantProc(float chance)
        {
            _chance = chance;
        }

        private readonly float _chance;

        public override bool Proc()
        {
            return Random.NextDouble() <= _chance;
        }
    }
}