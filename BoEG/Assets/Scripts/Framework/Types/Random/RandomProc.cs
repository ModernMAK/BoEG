using System;

namespace Framework.Types
{
    public abstract class RandomProc
    {
        protected RandomProc()
        {
            Random = new Random();
        }

        protected RandomProc(int seed)
        {
            Random = new Random(seed);
        }

        protected RandomProc(Random random)
        {
            Random = random;
        }

        protected Random Random { get; private set; }
        public abstract bool Proc();
    }
}