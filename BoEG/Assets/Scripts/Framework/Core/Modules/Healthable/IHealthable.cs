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

        float HealthCapacity { get; }
        float HealthGeneration { get; }

        event EventHandler<float> HealthChanged;
        event EventHandler<DeathEventArgs> Died;

        //Should  definately move this elsewhere; this will allow me to have the interface show this
        float BaseHealthCapacity { get; }
        float BonusHealthCapacity { get; }
        float BaseHealthGeneration { get; }
        float BonusHealthGeneration { get; }



    }
}