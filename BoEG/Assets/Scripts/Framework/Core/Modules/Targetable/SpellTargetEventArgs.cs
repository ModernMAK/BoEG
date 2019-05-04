using System;
using Framework.Types;
using UnityEngine;

namespace Framework.Core.Modules
{
    public class SpellTargetEventArgs : EventArgs
    {
        public SpellTargetEventArgs(Actor caster, float manaSpent, Damage damage)
        {
            Caster = caster;
            ManaSpent = manaSpent;
            Damage = damage;
        }
        
        public Actor Caster { get; private set; }
        public float ManaSpent { get; private set; }
        public Damage Damage { get; private set; }
    }
}