using System;

namespace MobaGame.Framework.Types
{
    [Flags]
    public enum LayerMaskFlag
    {
        Entity = 1 << Layer.Entity,
        World = 1 << Layer.World,
        Trigger = 1 << Layer.Trigger
    }
}