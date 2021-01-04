namespace MobaGame.Framework.Types
{
    /// <summary>
    ///     A Gradual Random;
    ///     P = (M+1) * C
    ///     This grantees a proc every 1/C tries, but consecutive procs are less likely.
    /// </summary>
    public class GradualProc : RandomProc
    {
        public GradualProc(float gradual)
        {
            Gradual = gradual;
            Misses = 0;
        }


        public float Gradual { get; }
        public int Misses { get; private set; }

        public float Chance => (Misses + 1) * Gradual;

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