using System;

namespace MobaGame.Framework.Core.Modules
{
	public interface IKillable
	{

        event EventHandler<DeathEventArgs> Died;
        /// <summary>
        /// A flag if the actor is currently dead.
        /// </summary>
        /// <returns>True if dead, false otherwise.</returns>
        bool IsDead { get; }
        void Die();
        void Die(Actor killer);
    }
}