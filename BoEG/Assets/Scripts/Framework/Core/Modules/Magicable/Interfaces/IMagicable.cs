using System;

namespace Framework.Core.Modules
{
    public interface IMagicable
    {
        
        float Mana { get; }
        float ManaPercentage { get; }
        float ManaCapacity { get; }
        float ManaGeneration { get; }
        void ModifyMana(float change);
        void SetMana(float mana);
        event EventHandler<MagicableEventArgs> Modified;
        event EventHandler<MagicableEventArgs> Modifying;
    }
}