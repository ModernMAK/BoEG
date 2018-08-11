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
}