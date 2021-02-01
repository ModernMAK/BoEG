using System;
using System.Collections.Generic;

namespace MobaGame.Framework.Core.Modules
{
	public static class ModifierX
    {
        public static FloatModifier SumModifiers<T>(this IEnumerable<T> enumerable, Func<T,FloatModifier> getter) where T : IModifier
		{
            var result = new FloatModifier(0, 0, 0);
            foreach(var item in enumerable)
			{
                var modifier = getter(item);
                result = result.AddModifier(modifier);
			}
            return result;

		}
    }

}