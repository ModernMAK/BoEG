using System;

namespace Components.Abilityable
{
    [Flags]
    public enum AbilityCast
    {
        NoTarget = 0x01,
        TargetPoint = 0x02,
        TargetRay = 0x04,
        TargetUnit = 0x08
    }
}