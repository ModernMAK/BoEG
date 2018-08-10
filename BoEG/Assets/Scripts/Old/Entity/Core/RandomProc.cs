using System;

namespace Old.Entity.Core
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
}