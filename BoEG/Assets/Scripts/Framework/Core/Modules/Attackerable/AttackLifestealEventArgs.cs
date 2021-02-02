using System;
using MobaGame.Framework.Types;

namespace MobaGame.Framework.Core.Modules
{
    public class AttackLifestealEventArgs : EventArgs
    {
        public AttackLifestealEventArgs(SourcedDamage<Actor> source)
        {
            Damage = source;
        }

        public SourcedDamage<Actor> Damage { get; }
        public float LifestealAmount { get; set; }
    }
}