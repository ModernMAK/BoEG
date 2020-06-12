using System;

namespace Framework.Core.Modules
{
    public interface IModifiable
    {
        event EventHandler<IModifier> OnModifierAdded;
        event EventHandler<IModifier> OnModifierRemoved;
    }
}