namespace Components.Armorable
{
    /// <summary>
    /// Data which is used to back IArmaorableInstance.
    /// </summary>
    public interface IArmorableData
    {
        /// <summary>
        /// The starting flat physical damage reduction.
        /// </summary>
        float BasePhysicalBlock { get; }

        /// <summary>
        /// The flat physical damage reduction gained per level.
        /// </summary>
        float GainPhysicalBlock { get; }

        /// <summary>
        /// The starting percentage-based physical damage reduction.
        /// </summary>
        float BasePhysicalResist { get; }

        /// <summary>
        /// The percentage-based physical damage reduction gained per level.
        /// </summary>
        float GainPhysicalResist { get; }

        /// <summary>
        /// Specifies whether physical damage is completely negated.
        /// </summary>
        bool HasPhysicalImmunity { get; }

        /// <summary>
        /// The starting flat magical damage reduction.
        /// </summary>
        float BaseMagicalBlock { get; }

        /// <summary>
        /// The flat magical damage reduction gained per level.
        /// </summary>
        float GainMagicalBlock { get; }

        /// <summary>
        /// The starting percentage-based physical damage reduction.
        /// </summary>
        float BaseMagicalResist { get; }

        /// <summary>
        /// The percentage-based magical damage reduction gained per level.
        /// </summary>
        float GainMagicalResist { get; }

        /// <summary>
        /// Specifies whether magical damage is completely negated.
        /// </summary>
        bool HasMagicalImmunity { get; }
    }
}