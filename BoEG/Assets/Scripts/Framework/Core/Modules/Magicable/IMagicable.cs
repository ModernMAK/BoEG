using System;

namespace MobaGame.Framework.Core.Modules
{
    public interface IMagicable
    {
        float Magic { get; set; }
        float MagicPercentage { get; set; }
        IModifiedValue<float> MagicCapacity { get; }
        IModifiedValue<float> MagicGeneration { get; }


        bool HasMagic(float mana);
        void SpendMagic(float mana);
        event EventHandler<float> MagicChanged;
    }

    public static class IMagicableExtensions
    {
        public static bool TrySpendMagic(this IMagicable magicable, float mana)
        {
            if (!magicable.HasMagic(mana)) return false;
            magicable.SpendMagic(mana);
            return true;

        }
    }
}