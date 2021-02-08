using System;

namespace MobaGame.Framework.Core.Modules
{
    public interface IMagicableView : IView
    {
        float Value { get; }
        float Percentage { get; }
        float Capacity { get; }
        float Generation { get; }
    }
    public interface IMagicable
    {
        IMagicableView View { get; }
        float Value { get; set; }
        float Percentage { get; set; }
        IModifiedValue<float> Capacity { get; }
        IModifiedValue<float> Generation { get; }


        bool HasMagic(float mana);
        void SpendMagic(float mana);
        event EventHandler<ChangedEventArgs<float>> ValueChanged;
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