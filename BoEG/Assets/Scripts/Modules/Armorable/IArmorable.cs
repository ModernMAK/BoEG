using Core;

namespace Modules.Armorable
{
    public interface IArmorable
    {
        Armor Physical { get; }
        Armor Magical { get; }

        Damage CalculateDamageAfterReductions(Damage damage);
        Damage ResistDamage(Damage damage);
        event DEFAULT_HANDLER Resisted;
    }
}