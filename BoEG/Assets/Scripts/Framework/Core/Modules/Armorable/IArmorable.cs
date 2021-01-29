using System;
using MobaGame.Framework.Types;

namespace MobaGame.Framework.Core.Modules
{
    public interface IArmorable
    {
        IArmor Physical { get; }
        IArmor Magical { get; }
        Damage ResistDamage(Damage damage);

        float CalculateReduction(Damage damage);

        event EventHandler<ArmorableEventArgs> Resisted;
        event EventHandler<ArmorableEventArgs> Resisting;
    }
}