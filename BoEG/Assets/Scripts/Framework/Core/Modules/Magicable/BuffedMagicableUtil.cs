namespace Framework.Core.Modules
{
    public static class BuffedMagicableUtil
    {
        private static IBuffList<T> CreateBuffList<T>(IBuffable buffable)
        {
            return new BuffList<T>(buffable);
        }

        private static IValueBuffCalculator CreateCalculator(IBuffList<IManaCapacityBonusBuff> buffList)
        {
            float Getter(IManaCapacityBonusBuff buff) => buff.ManaCapacityBonus;
            return new AdditiveBuffCalculator<IManaCapacityBonusBuff>(Getter, buffList);
        }

        private static IValueBuffCalculator CreateCalculator(IBuffList<IManaCapacityMultiplierBuff> buffList)
        {
            float Getter(IManaCapacityMultiplierBuff buff) => buff.ManaCapacityMultiplier;
            return new MultiplicativeBuffCalculator<IManaCapacityMultiplierBuff>(Getter, buffList);
        }

        private static IValueBuffCalculator CreateCalculator(IBuffList<IManaGenerationMultiplierBuff> buffList)
        {
            float Getter(IManaGenerationMultiplierBuff buff) => buff.ManaGenerationMultiplier;
            return new MultiplicativeBuffCalculator<IManaGenerationMultiplierBuff>(Getter, buffList);
        }

        private static IValueBuffCalculator CreateCalculator(IBuffList<IManaGenerationBonusBuff> buffList)
        {
            float Getter(IManaGenerationBonusBuff buff) => buff.ManaGenerationBonus;
            return new AdditiveBuffCalculator<IManaGenerationBonusBuff>(Getter, buffList);
        }

        public static void CreateManaGenerationBuff(IBuffable buffable, out IValueBuffCalculator add,
            out IValueBuffCalculator multiplier)
        {
            multiplier = CreateCalculator(CreateBuffList<IManaGenerationMultiplierBuff>(buffable));
            add = CreateCalculator(CreateBuffList<IManaGenerationBonusBuff>(buffable));
        }

        public static void CreateManaCapacityBuff(IBuffable buffable, out IValueBuffCalculator add,
            out IValueBuffCalculator multiplier)
        {
            multiplier = CreateCalculator(CreateBuffList<IManaCapacityMultiplierBuff>(buffable));
            add = CreateCalculator(CreateBuffList<IManaCapacityBonusBuff>(buffable));
        }
    }
}