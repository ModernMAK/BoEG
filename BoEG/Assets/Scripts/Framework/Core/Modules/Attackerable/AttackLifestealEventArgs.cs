using System;
using MobaGame.Framework.Types;

namespace MobaGame.Framework.Core.Modules
{
    public class AttackLifestealEventArgs : EventArgs
    {
        public AttackLifestealEventArgs(SourcedDamage source)
        {
            Damage = source;
        }

        public SourcedDamage Damage { get; }
        public float LifestealAmount { get; set; }
    }
}