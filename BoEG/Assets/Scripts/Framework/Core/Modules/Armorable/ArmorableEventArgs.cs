using System;
using Framework.Types;

namespace Framework.Core.Modules
{
    public class ArmorableEventArgs : EventArgs
    {
        public ArmorableEventArgs()
        {
            Damage = default;
            Reduction = default;
        }

        public Damage Damage { get; set; }
        public float Reduction { get; set; }
    }
}