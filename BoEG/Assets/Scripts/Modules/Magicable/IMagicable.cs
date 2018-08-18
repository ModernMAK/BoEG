using UnityEngine;

namespace Modules.Magicable
{
    public interface IMagicable
    {
        float ManaPercentage { get; } //Same as Mana Points
        float ManaPoints { get; } //Same as Mana Percentage
        float ManaCapacity { get; }
        float ManaGeneration { get; }
        void ModifyMana(float modification, GameObject source);
        event ManaModifiedHandler ManaModified;
    }
}