using System;

namespace Util
{
    public abstract class RandomProc
    {
        protected RandomProc()
        {
            Random = new Random();
        }

        protected Random Random { get; private set; }
        public abstract bool Proc();
    }

    public class DynamicProc : RandomProc
    {
        public DynamicProc() : this(0.5f)
        {
        }

        public DynamicProc(float chance)
        {
            _currentChance = chance;
        }

        private float _currentChance;

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