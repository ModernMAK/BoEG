using System;

namespace MobaGame.Framework.Core.Modules
{
	public interface IKillable
	{

        event EventHandler<DeathEventArgs> Died;
        bool IsDead { get; }
        void Die();
        void Die(Actor killer);
    }
}