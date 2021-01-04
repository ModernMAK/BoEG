namespace MobaGame.Framework.Types
{
    public class ConstantProc : RandomProc
    {
        private readonly float _chance;

        public ConstantProc(float chance)
        {
            _chance = chance;
        }

        public override bool Proc()
        {
            return Random.NextDouble() <= _chance;
        }
    }
}