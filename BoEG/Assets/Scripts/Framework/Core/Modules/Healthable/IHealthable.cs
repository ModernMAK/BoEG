using System;
using UnityEngine;

namespace MobaGame.Framework.Core.Modules
{

	public interface IHealthable
    {
        /// <summary>
        /// The Health Point Value.
        /// </summary>
        /// <remarks>
        /// This should always be between 0 and the Capacity Total.
        /// Changes to this will also change Percentage.
        /// </remarks>
        float Value { get; set; }
        /// <summary>
        /// The Health Point Value as a percentage of the Capacity Total.
        /// </summary>
        /// <remarks>
        /// This should always be between 0 and 1.
        /// Changes to this will also change Value.
        /// </remarks>
        float Percentage { get; set; }

        /// <summary>
        /// An interface exposing Health Capacity information.
        /// </summary>
        IModifiedValue<float> Capacity { get; }
        /// <summary>
        /// An interface exposing Health Generation information.
        /// </summary>
        IModifiedValue<float> Generation { get; }

        /// <summary>
        /// An event called after Value or Percentage changes.
        /// </summary>
        /// <remarks>
        /// The arguments are always the new and old Value.
        /// To determine percentage, cast sender to IHealthable and use Capacity.Total to calculate the percentage yourself.
        /// </remarks>
        event EventHandler<ChangedEventArgs<float>> ValueChanged;
        



    }
}