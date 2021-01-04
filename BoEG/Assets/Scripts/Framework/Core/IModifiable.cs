using System;

namespace MobaGame.Framework.Core
{
    public interface IModifiable
    {
        event EventHandler<IModifier> OnModifierAdded;
        event EventHandler<IModifier> OnModifierRemoved;
    }
}