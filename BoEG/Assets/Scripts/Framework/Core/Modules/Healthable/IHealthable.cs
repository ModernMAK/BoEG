using System;
using UnityEngine;

namespace MobaGame.Framework.Core.Modules
{
    public class DeathEventArgs : EventArgs
    {
        public DeathEventArgs(Actor actor)
        {
            Self = actor;
            GameObject = actor.gameObject;
        }

        public DeathEventArgs(GameObject go)
        {
            Self = go.GetComponent<Actor>();
            GameObject = go;
        }
        public Actor Self { get; }
        public GameObject GameObject { get; }
    }
    public interface IHealthable
    {
        float Health { get; set; }
        float HealthPercentage { get; set; }
        float HealthCapacity { get; set; }
        float HealthGeneration { get; set; }

        event EventHandler<float> HealthChanged;

        event EventHandler<DeathEventArgs> Died;

    }
}