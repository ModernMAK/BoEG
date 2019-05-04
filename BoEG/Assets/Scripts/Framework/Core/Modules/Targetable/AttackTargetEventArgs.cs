using System;
using Framework.Types;

namespace Framework.Core.Modules
{
    public class AttackTargetEventArgs : EventArgs
    {
        public AttackTargetEventArgs(Actor attacker, Damage damage)
        {
            Attacker = attacker;
            Damage = damage;
        }
        
        public Actor Attacker { get; private set; }
        public Damage Damage { get; private set; }
    }
}