using System;

namespace MobaGame.Framework.Core
{
    public interface IDynamicModifier : IModifier
	{
        event EventHandler Changed;
	}
    public interface IModifier
    {
    }
}