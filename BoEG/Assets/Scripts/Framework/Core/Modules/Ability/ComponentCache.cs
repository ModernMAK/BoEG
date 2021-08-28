using System;
using System.Collections.Generic;
using Framework.Core;
using UnityEngine;

namespace MobaGame.Framework.Core.Modules.Ability
{
    public class ComponentCache
    {
        public ComponentCache(GameObject gameObject)
        {
            _gameObject = gameObject;
            _transform = gameObject.transform;
            _cache = new Dictionary<Type, object>();
        }

        public ComponentCache(Transform transform)
        {
            _gameObject = transform.gameObject;
            _transform = transform;
            _cache = new Dictionary<Type, object>();
        }

        private readonly GameObject _gameObject;
        private readonly Transform _transform;
        private readonly Dictionary<Type, object> _cache;

        public GameObject GameObject => _gameObject;

        public Transform Transform => _transform;

        public bool TryGetCached<T>(out T value, bool allowProxy = true)
        {
            var type = typeof(T);
            if (_cache.TryGetValue(type, out var resultObj))
            {
                value = (T) resultObj;
                return true;
            }

            if (_gameObject.TryGetComponent(type, out var resultComp))
            {
                _cache[type] = resultComp;
                value = (T) (object) resultComp;
                return true;
            }

            if (allowProxy)
            {
                var proxyType = typeof(IProxy<T>);
                if (_gameObject.TryGetComponent(proxyType, out resultComp))
                {
                    var proxyComp = (IProxy<T>) resultComp;
                    _cache[type] = proxyComp.Value;
                    value = proxyComp.Value;
                    return true;
                }
            }

            value = default;
            return false;
        }

        public T GetCached<T>() => TryGetCached<T>(out var value) ? value : default;
    }
}