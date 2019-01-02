using Framework.Types;

namespace Framework.Core.Modules
{
    public interface IArmorable
    {
//        float PhysicalBlock { get; }
//        float PhysicalResist { get; }
//        bool PhysicalImmunity { get; }
//        float MagicalBlock { get; }
//        float MagicalResist { get; }
//        bool MagicalImmunity { get; }
        ArmorData Physical { get; }
        ArmorData Magical { get; }

//        float CalculateReduction(float value, DamageType type);
//        float CalculateDamageAfterReduction(float value, DamageType type);
//        Damage CalculateDamageAfterReduction(Damage damage);

        Damage ResistDamage(Damage damage);
        event ResistEventHandler ResistingDamage;
        event ResistEventHandler ResistedDamage;
    }
}