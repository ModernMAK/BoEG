﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using Entity.Abilities.FlameWitch;
using Framework.Core;
using Framework.Core.Modules;
using UnityEngine;

namespace Framework.Ability
{
    public class Abilitiable : MonoBehaviour, IAbilitiable, IInitializable<IReadOnlyList<IAbility>>
    {
        private IAbility[] _abilities;

        public bool FindAbility<T>(out T ability)
        {
            foreach (var temp in _abilities)
                if (temp is T result)
                {
                    ability = result;
                    return true;
                }

            ability = default;
            return false;
        }

        public IAbility GetAbility(int index)
        {
            return _abilities[index];
        }

        public int AbilityCount => _abilities.Length;

        public void Initialize(IReadOnlyList<IAbility> module)
        {
            var self = GetComponent<Actor>();
            _abilities = new IAbility[module.Count];
            for (var i = 0; i < _abilities.Length; i++) _abilities[i] = module[i];

            foreach (var ab in _abilities) ab.Initialize(self);
        }

        private event EventHandler<SpellEventArgs> _spellCasted;

        public event EventHandler<SpellEventArgs> SpellCasted
        {
            add => _spellCasted += value;
            remove => _spellCasted -= value;
        }

        public void NotifySpellCast(SpellEventArgs e) => OnSpellCast(e);

        protected virtual void OnSpellCast(SpellEventArgs e)
        {
            _spellCasted?.Invoke(this, e);
        }
    }
}