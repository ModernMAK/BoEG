using System;
using MobaGame.Framework.Types;

namespace MobaGame.Framework.Core.Modules
{
    public class AttackTargetEventArgs : EventArgs
    {
        public AttackTargetEventArgs(Actor attacker, Damage damage)
        {
            Attacker = attacker;
            Damage = damage;
        }

        public Actor Attacker { get; }
        public Damage Damage { get; }
    }
}