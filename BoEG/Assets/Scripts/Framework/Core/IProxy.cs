using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Core
{
    public interface IProxy<out T>
    {
        T Value { get; }
    }

    public static class ComponentManip
    {
        // [Obsolete("Use Actor IS/AS Cast")]
        public static bool TryGetModule<TModule>(this Component component, out TModule module) =>
            TryGetModule<TModule>(component.gameObject, out module);

        // [Obsolete("Use Actor IS/AS Cast")]
        public static bool TryGetModule<TModule>(this GameObject gameObject, out TModule module)
        {
            if (gameObject.TryGetComponent(out module))
            {
                return true;
            }

            if (gameObject.TryGetComponent<IProxy<TModule>>(out var proxy))
            {
                module = proxy.Value;
                return true;
            }

            module = default;
            return false;
        }

        // [Obsolete("Use Actor IS/AS Cast")]
        public static TModule GetModule<TModule>(this Component component) =>
            GetModule<TModule>(component.gameObject);
        
        // [Obsolete("Use Actor IS/AS Cast")]
        public static IList<TModule> GetModules<TModule>(this Component component) =>
            GetModules<TModule>(component.gameObject);

        // [Obsolete("Use Actor IS/AS Cast")]
        public static TModule GetModule<TModule>(this GameObject gameObject)
        {
            gameObject.TryGetModule<TModule>(out var result);
            return result;
        }

        // [Obsolete("Use Actor IS/AS Cast")]
        public static IList<TModule> GetModules<TModule>(this GameObject gameObject)
        {
            var modules = gameObject.GetComponents<TModule>();
            var proxied = gameObject.GetComponents<IProxy<TModule>>();
            var result = new TModule[modules.Length + proxied.Length];
            modules.CopyTo(result,0);
            for (var i = 0; i < proxied.Length; i++)
                result[i + modules.Length] = proxied[i].Value;
            return result;
        }

        // [Obsolete("Use Actor IS/AS Cast ")]
        public static TModule GetOrAddModule<TModule>(this Component component) where TModule : Component =>
            GetOrAddModule<TModule>(component.gameObject);

        public static TModule GetOrAddModule<TModule>(this GameObject gameObject) where TModule : Component
        {
            return gameObject.TryGetModule<TModule>(out var result) ? result : gameObject.AddModule<TModule>();
        }

        public static TModule GetOrAddModule<TModule, TProxy>(this Component component) where TProxy : Component, IProxy<TModule> =>
            GetOrAddModule<TModule, TProxy>(component.gameObject);

        public static TModule GetOrAddModule<TModule, TProxy>(this GameObject gameObject) where TProxy : Component, IProxy<TModule>
        {
            if (gameObject.TryGetModule<TModule>(out var result))
                return result;
            return gameObject.AddModule<TProxy>().Value;
        }

        public static TModule AddModule<TModule>(this Component component) where TModule : Component =>
            AddModule<TModule>(component.gameObject);

        public static TModule AddModule<TModule>(this GameObject gameObject) where TModule : Component
        {
            return gameObject.AddComponent<TModule>();
        }
    }
}