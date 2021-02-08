using System;
using MobaGame.Framework.Types;

namespace MobaGame.Framework.Core.Modules
{
    public class AttackCritEventArgs : EventArgs
    {
        public AttackCritEventArgs(SourcedDamage damage)
        {
            Damage = damage;
        }

        public SourcedDamage Damage { get; }

        /// <remarks>
        /// A value of 0 means no modifier is aplied.
        /// .5 would be a 150% chance crit
        /// </remarks>
        public float CriticalMultiplier { get; set; }

        public SourcedDamage FinalDamage =>
            Damage.SetDamage(Damage.Value.SetValue(Damage.Value.Value * (1f + CriticalMultiplier)));
    }
}