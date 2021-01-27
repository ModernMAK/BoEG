using System;
using MobaGame.Framework.Types;


namespace MobaGame.Framework.Core.Modules
{
    public class DamageEventArgs : EventArgs
    {
        public DamageEventArgs()
        {
            Source = default;
            Damage = default;
        }

        public DamageEventArgs(Actor source, Damage damage)
        {
            Source = source;
            Damage = damage;
        }

        public Actor Source { get; set; }
        public Damage Damage { get; set; }
    }
}