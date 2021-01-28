using System;

namespace MobaGame.Framework.Core.Modules
{
	public class Killable : ActorModule, IKillable, IRespawnable
	{
        public Killable(Actor actor) : base(actor)
		{
            IsDead = false;
		}
		public bool IsDead { get; private set; }

		public event EventHandler<DeathEventArgs> Died;

        protected void OnDied(DeathEventArgs args) => Died?.Invoke(this, args);

		public void Die()
		{
            OnDied(new DeathEventArgs(Actor));
            IsDead = true;
        }

        public void Die(Actor killer)
		{
            OnDied(new DeathEventArgs(Actor));
            IsDead = true;

		}

		public void Respawn()
		{
            IsDead = false;
		}
	}
}