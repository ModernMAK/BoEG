using System;
using UnityEngine;

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
    public interface IKillable
	{

        event EventHandler<DeathEventArgs> Died;
        bool IsDead { get; }
        void Die();
        void Die(Actor killer);
    }
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

	public interface IHealthable
    {
        float Health { get; set; }
        float HealthPercentage { get; set; }

        float HealthCapacity { get; }
        float HealthGeneration { get; }

        event EventHandler<float> HealthChanged;
        

        //Should  definately move this elsewhere; this will allow me to have the interface show this
        float BaseHealthCapacity { get; }
        float BonusHealthCapacity { get; }
        float BaseHealthGeneration { get; }
        float BonusHealthGeneration { get; }



    }
}