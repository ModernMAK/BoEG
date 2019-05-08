using System;

namespace Framework.Core.Modules
{
    public class MultiplicativeBuffCalculator<T> : ValueBuffCalculator<T>
    {
        public MultiplicativeBuffCalculator(Func<T, float> getter, IBuffList<T> buffList) : base(buffList)
        {
            Getter = getter;
        }


        private Func<T, float> Getter { get; set; }


        protected override void CalculateLogic()
        {
            var multiplier = 1f;
            foreach (var m in Buffs)
            {
                var val = Getter(m);

                multiplier *= (1 - val);
            }

            Value = multiplier;
        }
    }
}