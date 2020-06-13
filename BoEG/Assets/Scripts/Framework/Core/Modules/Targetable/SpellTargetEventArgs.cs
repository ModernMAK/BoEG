using System;
using Framework.Types;

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

        public Actor Caster { get; }
        public float ManaSpent { get; }
        public Damage Damage { get; }
    }
}