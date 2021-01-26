using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Core
{
    public static class ComponentManip
    {
        // [Obsolete("Use Actor IS/AS Cast")]

        [Obsolete("Use Actor TryGetModule")]
        public static bool TryGetModule<TModule>(this Component component, out TModule module) =>
			TryGetModule(component.gameObject, out module);

        // [Obsolete("Use Actor IS/AS Cast")]

        [Obsolete("Use Actor TryGetModule")]
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

        [Obsolete("Use Actor GetModule")]
        public static TModule GetModule<TModule>(this Component component) =>
            GetModule<TModule>(component.gameObject);
        
        [Obsolete("Use Actor GetModules")]
        // [Obsolete("Use Actor IS/AS Cast")]
        public static IList<TModule> GetModules<TModule>(this Component component) =>
            GetModules<TModule>(component.gameObject);

        [Obsolete("Use Actor GetModule")]
        // [Obsolete("Use Actor IS/AS Cast")]
        public static TModule GetModule<TModule>(this GameObject gameObject)
        {
            gameObject.TryGetModule<TModule>(out var result);
            return result;
        }

        [Obsolete("Use Actor GetModules")]
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

    }
}