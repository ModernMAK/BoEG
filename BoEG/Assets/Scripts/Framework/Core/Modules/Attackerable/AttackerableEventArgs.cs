using MobaGame.Framework.Types;
using System;

namespace MobaGame.Framework.Core.Modules
{
    public class AttackerableEventArgs : EventArgs
    {
        public AttackerableEventArgs(Actor attacker, Actor defender, Damage damage)
		{
            Attacker = attacker;
            Defender = defender;
            BaseDamage = damage;
            Damage = damage;
		}

        public Actor Attacker { get; }
        public Actor Defender { get; }
        public Damage BaseDamage { get; }
        public Damage Damage { get; set; }
    }
}