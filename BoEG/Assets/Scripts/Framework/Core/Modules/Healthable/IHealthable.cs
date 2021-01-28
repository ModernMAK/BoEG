using System;
using UnityEngine;

namespace MobaGame.Framework.Core.Modules
{

	public interface IHealthable
    {
        float Value { get; set; }
        float Percentage { get; set; }

        IModifiedValue<float> Capacity { get; }
        IModifiedValue<float> Generation { get; }

        event EventHandler<float> ValueChanged;
        



    }
}