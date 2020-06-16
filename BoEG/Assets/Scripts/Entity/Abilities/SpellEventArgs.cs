using System;

namespace Framework.Core.Modules
{
    public class SpellEventArgs : EventArgs
    {
        public Actor Caster { get; set; }
        public float ManaSpent { get; set; }
    }
}