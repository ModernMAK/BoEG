using System;
using UnityEngine;

namespace Modules.Abilityable.Ability
{
    [Serializable]
    public class AbilityLevel : Ability.AbilitySubmodule, IAbilityLevel
    {
        protected override void Initialize()
        {
            Level = 0;
        }

        [SerializeField] private int _maxLevel;

        public int Level { get; protected set; }

        public int MaxLevel
        {
            get { return _maxLevel; }
            private set { _maxLevel = value; }
        }


        public void LevelUp()
        {
            if (this.CanLevelUp())
                Level++;
        }

    }
    public interface IAbilityLevel
    {
        int Level { get; }
        int MaxLevel { get; }

        void LevelUp();
    }

    public static class IAbilityLevelExt
    {
        public static T GetLeveledData<T>(this IAbilityLevel self, T[] data, T def = default(T))
        {
            var lvl = self.Level;
            if (lvl <= 0 || data.Length == 0)
                return def; //Use default
            if (lvl >= data.Length)
                return data[data.Length - 1]; //Use highest value
            return data[lvl - 1]; //Evaluate
        }

        public static bool IsLeveled(this IAbilityLevel self)
        {
            return self.Level > 0;
        }

        public static bool CanLevelUp(this IAbilityLevel self)
        {
            return self.Level < self.MaxLevel;
        
        }
    }
}