using System;

namespace MobaGame.Framework.Core.Modules
{
	public class DeathEventArgs : EventArgs
    {

        public DeathEventArgs(Actor actor, Actor killer = null)
        {
            Self = actor;
            Killer = killer;
        }
        public Actor Self { get; }
        public Actor Killer { get; }
    }
}