namespace Util
{
    public class GradualProc : RandomProc
    {
        public GradualProc(float gradual)
        {
            Gradual = gradual;
            Misses = 0;
        }

        public float Gradual { get; private set; }
        public int Misses { get; private set; }

        public float Chance
        {
            get { return (Misses + 1) * Gradual; }
        }

        public override bool Proc()
        {
            var proc = Random.NextDouble() <= Chance;
            if (proc)
                Misses = 0;
            else
                Misses += 1;
            return proc;
        }
    }
}