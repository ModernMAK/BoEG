using System.Collections.Generic;
using UnityEngine;

namespace Entity
{
    public static class EntityRegistry
    {
        private static List<GameObject> _registry = new List<GameObject>();

        private static List<GameObject> Registry
        {
            get
            {
                if (_registry == null)
                    _registry = new List<GameObject>();

                return _registry;
            }
        }

        public static IEnumerable<GameObject> Registered
        {
            get { return Registry; }
        }

        public static void Register(GameObject go)
        {
            if (!_registry.Contains(go))
                _registry.Add(go);
        }

        public static void Deregister(GameObject go)
        {
            _registry.Remove(go);
        }
    }
}