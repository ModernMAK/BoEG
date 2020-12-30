using System;

namespace Framework.Core.Modules
{
    public interface IMagicable
    {
        float Magic { get; set; }
        float MagicPercentage { get; set; }
        float MagicCapacity { get; set; }
        float MagicGeneration { get; set; }


        bool HasMagic(float mana);
        void SpendMagic(float mana);
        event EventHandler<float> MagicChanged;
    }
}