//using System;
//using System.Collections.Generic;
//using Old.Modules.Buffable;
//using UnityEngine;
//
//namespace Old.Modules
//{
//    public static class ModuleUtil
//    {
//        public static float CalculateResist<T>(float resist, float resistGain, int deltaLevel,
//            IEnumerable<T> buffs, Func<T, float> func, bool clampToBase = false)
//        {
//            var coreResist = resist + resistGain * deltaLevel;
//            var actualResist = (1f - coreResist);
//            var final = actualResist;
//            foreach (var buff in buffs)
//            {
//                final *= (1f - func(buff));
//            }
//            //In this case, lower is better
//            if (clampToBase && final > actualResist)
//                final = actualResist;
//
//
//            return Mathf.Clamp(final, -1, 1f);
//        }
//
//        public static float CalculateResist<T>(this FullModule module, float resist, float resistGain,
//            Func<T, float> func, bool clampToBase = false)
//        {
//            var deltaLevel = module.GetLevel() - 1;
////            var buffs = module.GetBuffs<T>();
////            return CalculateResist(resist, resistGain, deltaLevel, buffs, func, clampToBase);
//            return 0f;
//        }
//        
//        
//        public static float CalculateValue(float baseValue, float gainRate, int levelDelta, bool clampToBase = false)
//        {
//            var final = (baseValue + gainRate * levelDelta);
//            if (clampToBase && final < baseValue)
//                final = baseValue;
//            return final;
//        }
//
//        public static float CalculateValue(this LeveledModule module, float baseValue, float gainRate,
//            bool clampToBase = false)
//        {
//            var levelDelta = module.GetLevel() - 1;
//            return CalculateValue(baseValue, gainRate, levelDelta, clampToBase);
//        }
//
//        public static float CalculateValue<T>(float baseValue, Func<T, float> bonusFunc, Func<T, float> multFunc,
//            IEnumerable<T> buffs, bool clampToBase = false) where T : IBuffInstance
//        {
//            var multiplier = 1f;
//            var bonus = 0f;
//            foreach (var buff in buffs)
//            {
//                bonus += bonusFunc(buff);
//                multiplier *= (1f - multFunc(buff));
//            }
//            var final = baseValue * multiplier + bonus;
//            if (clampToBase && final < baseValue)
//                final = baseValue;
//            return final;
//        }
//
//        public static float CalculateValue<T>(this BuffedModule module, float baseValue, Func<T, float> bonusFunc,
//            Func<T, float> multFunc, bool clampToBase = false) where T : IBuffInstance
//        {
//            var buffs = module.GetBuffs<T>();
//            return CalculateValue(baseValue, bonusFunc, multFunc, buffs, clampToBase);
//        }
//
//        public static float CalculateValueBonus<T>(float baseValue, Func<T, float> bonusFunc, IEnumerable<T> buffs,
//            bool clampToBase = false) where T : IBuffInstance
//        {
//            var bonus = 0f;
//            foreach (var buff in buffs)
//            {
//                bonus += bonusFunc(buff);
//            }
//            var final = baseValue + bonus;
//            if (clampToBase && final < baseValue)
//                final = baseValue;
//            return final;
//        }
//
//        public static float CalculateValueBonus<T>(this BuffedModule module, float baseValue, Func<T, float> bonusFunc,
//            bool clampToBase = false) where T : IBuffInstance
//        {
//            var buffs = module.GetBuffs<T>();
//            return CalculateValueBonus(baseValue, bonusFunc, buffs, clampToBase);
//        }
//
//        public static float CalculateValueMultiplier<T>(float baseValue, Func<T, float> multFunc, IEnumerable<T> buffs,
//            bool clampToBase = false) where T : IBuffInstance
//        {
//            var multiplier = 1f;
//            foreach (var buff in buffs)
//            {
//                multiplier *= (1f - multFunc(buff));
//            }
//            var final = baseValue * multiplier;
//            if (clampToBase && final < baseValue)
//                final = baseValue;
//            return final;
//        }
//
//        public static float CalculateValue<T>(this BuffedModule module, float baseValue, Func<T, float> multFunc,
//            bool clampToBase = false) where T : IBuffInstance
//        {
//            var buffs = module.GetBuffs<T>();
//            return CalculateValueMultiplier(baseValue, multFunc, buffs, clampToBase);
//        }
//
//        public static float CalculateValueBonus<T>(float baseValue, float gainRate, int levelDelta,
//            Func<T, float> bonusFunc, IEnumerable<T> buffs, bool clampToBase = false) where T : IBuffInstance
//        {
//            var gainTotal = gainRate * levelDelta;
//            var bonus = 0f;
//            foreach (var buff in buffs)
//            {
//                bonus += bonusFunc(buff);
//            }
//            var final = (baseValue + gainTotal) + bonus;
//            if (clampToBase && final < baseValue)
//                final = baseValue;
//            return final;
//        }
//
//        public static float CalculateValueBonus<T>(this FullModule module, float baseValue, float gainRate,
//            Func<T, float> bonusFunc, bool clampToBase = false)
//        {
//            var levelDelta = module.GetLevel() - 1;
////            var buffs = module.GetBuffs<T>();
////            return CalculateValueBonus(baseValue, gainRate, levelDelta, bonusFunc, buffs, clampToBase);
//            return 0f;
//        }
//
//        public static float CalculateValueMultiplier<T>(float baseValue, float gainRate, int levelDelta,
//            Func<T, float> multFunc, IEnumerable<T> buffs, bool clampToBase = false) where T : IBuffInstance
//        {
//            var gainTotal = gainRate * levelDelta;
//            var multiplier = 1f;
//            foreach (var buff in buffs)
//            {
//                multiplier *= (1f - multFunc(buff));
//            }
//            var final = (baseValue + gainTotal) * multiplier;
//            if (clampToBase && final < baseValue)
//                final = baseValue;
//            return final;
//        }
//
//        public static float CalculateValueMultiplier<T>(this FullModule module, float baseValue, float gainRate,
//            Func<T, float> multFunc, bool clampToBase = false) where T : IBuffInstance
//        {
//            var levelDelta = module.GetLevel() - 1;
//            var buffs = module.GetBuffs<T>();
//            return CalculateValueMultiplier(baseValue, gainRate, levelDelta, multFunc, buffs, clampToBase);
//        }
//
//        public static float CalculateValue<T>(float baseValue, float gainRate, int levelDelta, Func<T, float> bonusFunc,
//            Func<T, float> multFunc, IEnumerable<T> buffs, bool clampToBase = false) where T : IBuffInstance
//        {
//            var gainTotal = gainRate * levelDelta;
//            var multiplier = 1f;
//            var bonus = 0f;
//            foreach (var buff in buffs)
//            {
//                bonus += bonusFunc(buff);
//                multiplier *= (1f - multFunc(buff));
//            }
//            var final = (baseValue + gainTotal) * multiplier + bonus;
//            if (clampToBase && final < baseValue)
//                final = baseValue;
//            return final;
//        }
//
//        public static float CalculateValue<T>(this FullModule module, float baseValue, float gainRate,
//            Func<T, float> bonusFunc, Func<T, float> multFunc, bool clampToBase = false) where T : IBuffInstance
//        {
//            var levelDelta = module.GetLevel() - 1;
//            var buffs = module.GetBuffs<T>();
//            return CalculateValue(baseValue, gainRate, levelDelta, bonusFunc, multFunc, buffs, clampToBase);
//        }
//    }
//}