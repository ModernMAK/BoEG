using System;

namespace MobaGame.Framework.Core.Modules
{
    public class AbilityEventArgs : EventArgs
    {
        public AbilityEventArgs(Actor caster, float manaSpent = default)
        {
            Caster = caster;
            ManaSpent = manaSpent;
        }
        public Actor Caster { get; }
        public float ManaSpent { get; }
    }
}