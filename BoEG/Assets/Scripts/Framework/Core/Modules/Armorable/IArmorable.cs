using System;
using MobaGame.Framework.Types;

namespace MobaGame.Framework.Core.Modules
{
    public interface IArmorableView : IView
    {
        IArmorView Physical { get; }
        IArmorView Magical { get; }
    }
    public interface IArmorable
    {
        IArmorableView View { get; }
        IArmor Physical { get; }
        IArmor Magical { get; }
        Damage ResistDamage(Damage damage);

        float CalculateReduction(Damage damage);

        event EventHandler<ArmorableEventArgs> Resisted;
        event EventHandler<ArmorableEventArgs> Resisting;
    }
}