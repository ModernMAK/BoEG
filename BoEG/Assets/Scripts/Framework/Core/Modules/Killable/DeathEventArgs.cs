using System;

namespace MobaGame.Framework.Core.Modules
{
	public class DeathEventArgs : EventArgs
    {
        public DeathEventArgs(Actor actor) : this(actor, null) { }

        public DeathEventArgs(Actor actor, Actor killer)
        {
            Self = actor;
            Killer = killer;
        }
        public Actor Self { get; }
        public Actor Killer { get; }
    }
}