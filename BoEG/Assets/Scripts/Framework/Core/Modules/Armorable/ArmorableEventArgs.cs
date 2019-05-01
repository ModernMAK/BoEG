using System;
using Framework.Types;

namespace Framework.Core.Modules
{
    public class ArmorableEventArgs : EventArgs
    {
        public ArmorableEventArgs(Damage damage, float reduction)
        {
            Damage = damage;
            Reduction = reduction;
        }

        public Damage Damage { get; private set; }
        public float Reduction { get; private set; }
    }
}