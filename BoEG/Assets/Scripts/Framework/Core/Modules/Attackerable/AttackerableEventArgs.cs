using MobaGame.Framework.Types;
using System;

namespace MobaGame.Framework.Core.Modules
{
    public class AttackerableEventArgs : EventArgs
    {
        public AttackerableEventArgs(SourcedDamage damage, Actor defender)
		{
            Defender = defender;
            Damage = damage;
		}

        public Actor Attacker => Damage.Source;
        public Actor Defender { get; }
        public SourcedDamage Damage { get; }
        public Damage DamageValue => Damage.Value;
    }
}