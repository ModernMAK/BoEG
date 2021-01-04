namespace MobaGame.Framework.Types
{
    public class DynamicProc : RandomProc
    {
        private float _currentChance;

        public DynamicProc() : this(0.5f)
        {
        }

        public DynamicProc(float chance)
        {
            _currentChance = chance;
        }

        public override bool Proc()
        {
            return Random.NextDouble() <= _currentChance;
        }

        public bool Proc(float chance)
        {
            _currentChance = chance;
            return Proc();
        }
    }
}