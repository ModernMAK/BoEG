using System;

namespace Framework.Types
{
    [Flags]
    public enum DamageModifiers
    {
        None = 0,
        Attack,
        Ability,
        Reflected
    }
}