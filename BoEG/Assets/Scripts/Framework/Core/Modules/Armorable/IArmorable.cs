using System;
using MobaGame.Framework.Types;

namespace MobaGame.Framework.Core.Modules
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
    public static class IArmorableX
    {
        public static SourcedDamage ResistDamage(this IArmorable armorable, SourcedDamage damage) => damage.SetDamage(armorable.ResistDamage(damage.Value));
    }

}