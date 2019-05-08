namespace Framework.Core.Modules
{
    public static class BuffedHealthableUtil
    {
        private static IBuffList<T> CreateBuffList<T>(IBuffable buffable)
        {
            return new BuffList<T>(buffable);
        }

        private static IValueBuffCalculator CreateCalculator(IBuffList<IHealthCapacityBonusBuff> buffList)
        {
            float Getter(IHealthCapacityBonusBuff buff) => buff.HealthCapacityBonus;
            return new AdditiveBuffCalculator<IHealthCapacityBonusBuff>(Getter, buffList);
        }

        private static IValueBuffCalculator CreateCalculator(IBuffList<IHealthCapacityMultiplierBuff> buffList)
        {
            float Getter(IHealthCapacityMultiplierBuff buff) => buff.HealthCapacityMultiplier;
            return new MultiplicativeBuffCalculator<IHealthCapacityMultiplierBuff>(Getter, buffList);
        }

        private static IValueBuffCalculator CreateCalculator(IBuffList<IHealthGenerationMultiplierBuff> buffList)
        {
            float Getter(IHealthGenerationMultiplierBuff buff) => buff.HealthGenerationMultiplier;
            return new MultiplicativeBuffCalculator<IHealthGenerationMultiplierBuff>(Getter, buffList);
        }

        private static IValueBuffCalculator CreateCalculator(IBuffList<IHealthGenerationBonusBuff> buffList)
        {
            float Getter(IHealthGenerationBonusBuff buff) => buff.HealthGenerationBonus;
            return new AdditiveBuffCalculator<IHealthGenerationBonusBuff>(Getter, buffList);
        }

        public static void CreateHealthGenerationBuff(IBuffable buffable, out IValueBuffCalculator add,
            out IValueBuffCalculator multiplier)
        {
            multiplier = CreateCalculator(CreateBuffList<IHealthGenerationMultiplierBuff>(buffable));
            add = CreateCalculator(CreateBuffList<IHealthGenerationBonusBuff>(buffable));
        }

        public static void CreateHealthCapacityBuff(IBuffable buffable, out IValueBuffCalculator add,
            out IValueBuffCalculator multiplier)
        {
            multiplier = CreateCalculator(CreateBuffList<IHealthCapacityMultiplierBuff>(buffable));
            add = CreateCalculator(CreateBuffList<IHealthCapacityBonusBuff>(buffable));
        }
    }
}