using System;
using System.Collections.Generic;

namespace MobaGame.Framework.Core.Modules
{
	public static class ModifierX
    {
        public static Modifier SumModifiers<T>(this IEnumerable<T> enumerable, Func<T,Modifier> getter) where T : IModifier
		{
            var result = new Modifier(0, 0, 0);
            foreach(var item in enumerable)
			{
                var modifier = getter(item);
                result = result.AddModifier(modifier);
			}
            return result;

		}
    }

}