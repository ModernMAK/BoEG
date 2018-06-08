using System;
using Core;

namespace Components.Magicable
{
    public interface IMagicableInstance 
    {
        float ManaPoints { get; set; }
        float ManaRatio { get; set; }
        float ManaCapacity { get; }
        float ManaGen { get; }
    }
}