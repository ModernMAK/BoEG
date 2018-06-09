using Components.Buffable;

namespace Components.Armorable
{
    public interface IArmorableBuffInstance : IBuffInstance
    {
        /// <summary>
        /// A modifier to physical block. When applicable, these buffs stack additevely.
        /// </summary>
        float PhysicalBlockBonus { get; }
        
        /// <summary>
        /// A modifier to physical resist. When applicable, these buffs stack multiplicatively.
        /// </summary>
        float PhysicalResistkMultiplier { get; }
        
        /// <summary>
        /// Grants physical immunity when present.
        /// </summary>
        bool ProvidePhysicalImmunity { get; }

        /// <summary>
        /// A modifier to magical block. When applicable, these buffs stack additevely.
        /// </summary>
        float MagicalBlockBonus { get; }
        
        /// <summary>
        /// A modifier to magical resist. When applicable, these buffs stack multiplicatively.
        /// </summary>
        float MagicalResistkMultiplier { get; }
        
        /// <summary>
        /// Grants magical immunity when present.
        /// </summary>
        bool ProvideMagicalImmunity { get; }
    }
}