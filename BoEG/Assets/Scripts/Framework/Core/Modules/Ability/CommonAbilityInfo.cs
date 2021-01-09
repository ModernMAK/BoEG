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

    public class ModuleCache : ComponentCache
    {
        //Helper functions
        public IHealthable Healthable => GetCached<IHealthable>();
        public IMagicable Magicable => GetCached<IMagicable>();
        public IAbilitiable Abilitiable => GetCached<IAbilitiable>();
        public ITeamable Teamable => GetCached<ITeamable>();

        public IMovable Movable => GetCached<IMovable>();

        public IAttackerable Attackerable => GetCached<IAttackerable>();

        public ICommandable Commandable => GetCached<ICommandable>();

        public ModuleCache(GameObject gameObject) : base(gameObject)
        {
        }

        public ModuleCache(Transform transform) : base(transform)
        {
        }
    }


    public class CommonAbilityInfo
    {
        private readonly Actor _actor;
        private readonly Transform _transform;
        private readonly ITeamable _teamable;
        private readonly IMagicable _magicable;
        private readonly IAbilitiable _abilitiable;
        public IMagicable Magicable => _magicable;

        public ITeamable Teamable => _teamable;

        public IAbilitiable Abilitiable => _abilitiable;
        public Transform Transform => _transform;
        public float Range { get; set; }

        public CommonAbilityInfo(Actor actor)
        {
            _magicable = actor.GetModule<IMagicable>();
            _teamable = actor.GetModule<ITeamable>();
            _abilitiable = actor.GetModule<IAbilitiable>();
            _transform = actor.transform;
            _actor = actor;
        }

        public float ManaCost { get; set; }

        public bool TrySpendMana()
        {
            if (HasMana())
            {
                SpendMana();
                return true;
            }

            return false;
        }

        public bool HasMana() => _magicable.HasMagic(ManaCost);

        public void SpendMana() => _magicable.SpendMagic(ManaCost);

        public bool SameTeam(GameObject go, bool defaultValue = false) =>
            SameTeam(go.GetComponent<ITeamable>(), defaultValue);

        public bool SameTeam(ITeamable teamable, bool defaultValue = false)
        {
            if (_teamable == null || teamable == null)
                return defaultValue;
            return _teamable.SameTeam(teamable);
        }

        public bool InRange(Transform transform) => AbilityHelper.InRange(_transform, transform.position, Range);
        public bool InRange(Vector3 position) => AbilityHelper.InRange(_transform, position, Range);


        public void NotifySpellCast(SpellEventArgs args) => _abilitiable.NotifySpellCast(args);

        public void NotifySpellCast() =>
            _abilitiable.NotifySpellCast(new SpellEventArgs() {Caster = _actor, ManaSpent = ManaCost});
    }
}