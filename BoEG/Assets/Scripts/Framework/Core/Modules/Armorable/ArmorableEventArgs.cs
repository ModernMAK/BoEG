using System;
using Framework.Types;
using UnityEngine;

namespace Framework.Core.Modules
{
    public static class ArmorableEventArgsUtilities
    {
        public static float CalculateReduction(this ArmorableEventArgs args)
        {
            return args.OutgoingDamage.Value - args.IncomingDamage.Value;
        }
        
        public static ArmorableEventArgs CreateNextArmorableArgs(this ArmorableEventArgs args) => new ArmorableEventArgs(args.OutgoingDamage);
    }

    public class ArmorableEventArgs : EventArgs
    {
        public ArmorableEventArgs(Damage damage = default) : this(damage, damage)
        {
        }

        public ArmorableEventArgs(Damage incoming, Damage outgoing)
        {
            IncomingDamage = incoming;
            OutgoingDamage = outgoing;
        }

        /// <summary>
        /// The damage coming into the Armorable event
        /// </summary>
        public Damage IncomingDamage { get; }

        public Damage OutgoingDamage { get; set; }

    }
}