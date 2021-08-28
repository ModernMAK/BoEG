using System;
using MobaGame.Framework.Types;

namespace MobaGame.Framework.Core.Modules
{
    public interface IArmorable
    {
        IArmorableView View { get; }
        IArmor Physical { get; }
        IArmor Magical { get; }
        SourcedDamage ResistDamage(SourcedDamage damage);

        float CalculateReduction(SourcedDamage damage);

        event EventHandler<ChangedEventArgs<SourcedDamage>> Resisted;
        event EventHandler<SourcedDamage> Resisting;
        event EventHandler<ChangableEventArgs<SourcedDamage>> DamageMitigation;
    }
}