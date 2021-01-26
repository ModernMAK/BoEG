using System.Collections;
using System.Collections.Generic;

namespace MobaGame.Framework.Core
{
	public static class EnumerableQuery
    {
        

        public static T Get<T>(IEnumerable enumerable)
        {
            TryGet<T>(enumerable, out var module);
            return module;
        }

        public static bool TryGet<T>(IEnumerable enumerable, out T module)
        {
            foreach (var m in enumerable)
            {
                if (m is T result)
                {
                    module = result;
                    return true;
                }
            }

            module = default;
            return false;
        }

        public static IEnumerable<T> GetAll<T>(IEnumerable enumerable)
        {
            foreach (var item in enumerable)
            {
                if (item is T result)
                {
                    yield return result;
                }
            }
        }

        public static IReadOnlyList<T> GetAllAsList<T>(IEnumerable enumerable)
        {
            var list = new List<T>();
            foreach (var item in enumerable)
            {
                if (item is T result)
                {
                    list.Add(result);
                }
            }

            return list;
        }
        
    }
}