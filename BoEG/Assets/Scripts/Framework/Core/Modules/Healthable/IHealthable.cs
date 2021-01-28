using System;
using UnityEngine;

namespace MobaGame.Framework.Core.Modules
{

	public interface IHealthable
    {
        float Health { get; set; }
        float HealthPercentage { get; set; }

        IModifiedValue<float> HealthCapacity { get; }
        IModifiedValue<float> HealthGeneration { get; }

        event EventHandler<float> HealthChanged;
        



    }
}