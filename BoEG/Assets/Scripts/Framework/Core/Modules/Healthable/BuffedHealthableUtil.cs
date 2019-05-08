namespace Framework.Core.Modules
{
    public static class BuffedHealthableUtil
    {
        private static IBuffList<T> CreateBuffList<T>(this IBuffable buffable)
        {
            return new BuffList<T>(buffable);
        }

        public static IValueBuffCalculator CreateCalculator(this IBuffList<IHealthCapacityBonusBuff> buffList)
        {
            float Getter(IHealthCapacityBonusBuff buff) => buff.HealthCapacityBonus;
            return new AdditiveBuffCalculator<IHealthCapacityBonusBuff>(Getter, buffList);
        }

        public static IValueBuffCalculator CreateCalculator(this IBuffList<IHealthCapacityMultiplierBuff> buffList)
        {
            float Getter(IHealthCapacityMultiplierBuff buff) => buff.HealthCapacityMultiplier;
            return new MultiplicativeBuffCalculator<IHealthCapacityMultiplierBuff>(Getter, buffList);
        }

        public static IValueBuffCalculator CreateCalculator(this IBuffList<IHealthGenerationMultiplierBuff> buffList)
        {
            float Getter(IHealthGenerationMultiplierBuff buff) => buff.HealthGenerationMultiplier;
            return new MultiplicativeBuffCalculator<IHealthGenerationMultiplierBuff>(Getter, buffList);
        }

        public static IValueBuffCalculator CreateCalculator(this IBuffList<IHealthGenerationBonusBuff> buffList)
        {
            float Getter(IHealthGenerationBonusBuff buff) => buff.HealthGenerationBonus;
            return new AdditiveBuffCalculator<IHealthGenerationBonusBuff>(Getter, buffList);
        }

        public static void CreateHealthGenerationBuff(this IBuffable buffable, out IValueBuffCalculator add,
            out IValueBuffCalculator multiplier)
        {
            multiplier = buffable.CreateBuffList<IHealthGenerationMultiplierBuff>().CreateCalculator();
            add = buffable.CreateBuffList<IHealthGenerationBonusBuff>().CreateCalculator();
        }

        public static void CreateHealthCapacityBuff(this IBuffable buffable, out IValueBuffCalculator add,
            out IValueBuffCalculator multiplier)
        {
            multiplier = buffable.CreateBuffList<IHealthCapacityMultiplierBuff>().CreateCalculator();
            add = buffable.CreateBuffList<IHealthCapacityBonusBuff>().CreateCalculator();
        }
    }
}