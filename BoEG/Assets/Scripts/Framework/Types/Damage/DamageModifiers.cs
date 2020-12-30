using System;

namespace Framework.Types
{
    [Flags]
    public enum DamageModifiers
    {
        None = 0,
        Attack = 1 << 0,
        Ability = 1 << 1,
        Reflected = 1 << 2
    }
}