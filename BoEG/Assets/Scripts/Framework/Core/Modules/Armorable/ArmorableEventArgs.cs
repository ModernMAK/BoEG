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

        /// <summary>
        /// A copy of the damage instance to reduce.
        /// </summary>
        public Damage Damage { get; set; }
        /// <summary>
        /// The amount of damage we are reducing. 
        /// </summary>
        public float Reduction { get; set; }
    }
}