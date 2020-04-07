using System;

namespace Framework.Core.Modules
{
    public class AdditiveBuffCalculator<T> : ValueBuffCalculator<T>
    {
        public AdditiveBuffCalculator(Func<T, float> getter, IBuffList<T> buffList) : base(buffList)
        {
            Getter = getter;
        }


        private Func<T, float> Getter { get; set; }

        protected override void CalculateLogic()
        {
            var bonus = 0f;
            foreach (var m in Buffs)
            {
                var val = Getter(m);
                bonus += val;
            }

            Value = bonus;
        }
    }
}