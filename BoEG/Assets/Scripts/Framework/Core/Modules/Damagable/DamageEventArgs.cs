using System;
using MobaGame.Framework.Types;
using UnityEngine;


namespace MobaGame.Framework.Core.Modules
{
    public class DamageEventArgs : EventArgs
    {
        public DamageEventArgs()
        {
            Source = default;
            Damage = default;
        }

        public DamageEventArgs(GameObject source, Damage damage)
        {
            Source = source;
            Damage = damage;
        }

        public GameObject Source { get; set; }
        public Damage Damage { get; set; }
    }
}