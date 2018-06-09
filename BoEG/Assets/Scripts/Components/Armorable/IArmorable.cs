namespace Components.Armorable
{
    public interface IArmorable
    {
        /// <summary>
        /// The current flat physical damage negation.
        /// </summary>
        float PhysicalBlock { get; }

        /// <summary>
        /// The current percentage-based physical damage negation.
        /// </summary>
        float PhysicalResist { get; }

        /// <summary>
        /// Whether all physical damage should be negated.
        /// </summary>
        bool PhysicalImmunity { get; }

        /// <summary>
        /// The current flat magical damage negation.
        /// </summary>
        float MagicalBlock { get; }

        /// <summary>
        /// The current percentage-based magical damage negation.
        /// </summary>
        float MagicalResist { get; }

        /// <summary>
        /// Whether all magical damage should be negated.
        /// </summary>
        bool MagicalImmunity { get; }

        /// <summary>
        /// Calculates incoming damage after reductions.
        /// </summary>
        /// <param name="damage">The incoming damage.</param>
        /// <param name="type">The type of incoming damage.</param>
        /// <returns>Damage after reductions.</returns>
        float CalculateReduction(float damage, DamageType type);
    }
}