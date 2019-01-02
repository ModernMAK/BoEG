using System;
using Framework.Types;
using UnityEngine;

namespace Framework.Core.Modules
{
    public class SpellTargetEventArgs : EventArgs
    {
        public SpellTargetEventArgs(GameObject caster, float manaSpent, Damage damage)
        {
            Caster = caster;
            ManaSpent = manaSpent;
            Damage = damage;
        }
        
        public GameObject Caster { get; private set; }
        public float ManaSpent { get; private set; }
        public Damage Damage { get; private set; }
    }
}