using System;
using MobaGame.Framework.Types;

namespace MobaGame.Framework.Core.Modules
{
    public class AttackCritEventArgs : EventArgs
    {
        public AttackCritEventArgs(SourcedDamage<Actor> damage)
        {
            Damage = damage;
        }

        public SourcedDamage<Actor> Damage { get; }

        /// <remarks>
        /// A value of 0 means no modifier is aplied.
        /// .5 would be a 150% chance crit
        /// </remarks>
        public float CriticalMultiplier { get; set; }

        public SourcedDamage<Actor> FinalDamage =>
            Damage.SetDamage(Damage.Damage.SetValue(Damage.Damage.Value * (1f + CriticalMultiplier)));
    }
}