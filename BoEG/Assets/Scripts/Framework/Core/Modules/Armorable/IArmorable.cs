using System;
using Framework.Types;

namespace Framework.Core.Modules
{
    public interface IArmorable
    {
        Armor Physical { get; }
        Armor Magical { get; }
        Damage ResistDamage(Damage damage);

        float CalculateReduction(Damage damage);

        event EventHandler<ArmorableEventArgs> Resisted;
        event EventHandler<ArmorableEventArgs> Resisting;
    }
}