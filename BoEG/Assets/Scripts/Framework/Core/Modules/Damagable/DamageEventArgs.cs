using System;
using Framework.Types;

namespace Framework.Core.Modules
{
    public class DamageEventArgs : EventArgs
    {
        public DamageEventArgs(Damage damage)
        {
            Damage = damage;
        }

        public Damage Damage { get; private set; }
    }
}