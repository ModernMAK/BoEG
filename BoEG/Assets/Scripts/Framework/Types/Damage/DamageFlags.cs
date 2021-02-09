using System;

namespace MobaGame.Framework.Types
{
    [Flags]
    public enum DamageFlags : byte
    {
        None = 0,
        Attack = 1 << 0,
        Ability = 1 << 1,
        Reflected = 1 << 2
    }
}