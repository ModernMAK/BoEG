using System.Collections.Generic;

namespace MobaGame.Framework.Core
{
	public static class ActorUtil
    {
        public static T GetModuleOrComponent<T>(this Actor actor)
        {
            if (actor.TryGetModule<T>(out var module))
                return module;
            if (actor.TryGetComponent<T>(out var component))
                return component;
            return default;
        }

        public static bool TryGetModuleOrComponent<T>(this Actor actor, out T module)
        {
            if (actor.TryGetModule<T>(out var moduleResult))
            {
                module = moduleResult;
                return true;
            }

            if (actor.TryGetComponent<T>(out var componentResult))
            {
                module = componentResult;
                return true;
            }

            module = default;
            return default;
        }

        public static IEnumerable<T> GetModules<T>(this Actor actor)
        {
            var modules = actor.GetModules<T>();
            foreach (var module in modules)
            {
                yield return module;
            }

            var components = actor.GetComponents<T>();
            foreach (var component in components)
            {
                yield return component;
            }
        }

        public static IReadOnlyList<T> GetModulesAsList<T>(this Actor actor)
        {
            var modules = actor.GetModules<T>();
            var components = actor.GetComponents<T>();
            var list = new List<T>(components);
            list.AddRange(modules);
            return list;
        }
    }
}