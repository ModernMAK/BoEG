using System;
using System.Collections.Generic;
using MobaGame.Framework.Core.Modules.Ability;

namespace MobaGame.Framework.Core.Modules
{
    public interface IAbilitiable
    {

        /// <summary>
        /// Try and get the ability of the specified type.
        /// </summary>
        /// <param name="ability">The reference to the found ability.</param>
        /// <typeparam name="TAbility">The type of ability.</typeparam>
        /// <returns>True if an ability was found, false otherwise.</returns>
        /// <remarks>Intended to be used to get a specific ability, E.G. to get FlameWitch's Overheat to check if it is active.</remarks>
        bool TryGetAbility<TAbility>(out TAbility ability) where TAbility : IAbility;

        /// <summary>
        /// The abilities of this instance.
        /// </summary>
        IReadOnlyList<IAbility> Abilities { get; }

        /// <summary>
        /// Called when an ability is casted.
        /// </summary>
        event EventHandler<AbilityEventArgs> AbilityCasted;

        /// <summary>
        /// Called when the Abilities list changes.
        /// </summary>
        event EventHandler AbilitiesChanged;

        /// <summary>
        /// Raises the Ability Casted Event safely.
        /// </summary>
        /// <param name="e">The event args.</param>
        /// <remarks>
        /// Should only be used from abilities.
        /// Maybe moved to a callback in the future.
        /// </remarks>
        void NotifySpellCast(AbilityEventArgs e);
    }
}