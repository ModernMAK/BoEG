using System;
using System.Collections.Generic;
using MobaGame.Framework.Core.Modules.Ability;
using UnityEngine;

namespace MobaGame.Framework.Core.Modules
{
    public class AbilitiableModule : MonoBehaviour, IAbilitiable, IInitializable<IReadOnlyList<IAbility>>
    {
        [SerializeField] private Abilitiable _abilitiable;

        private void Awake()
        {
            _abilitiable = new Abilitiable(GetComponent<Actor>());
        }

        public bool FindAbility<T>(out T ability) => _abilitiable.FindAbility(out ability);


        public IAbility GetAbility(int index) => _abilitiable.GetAbility(index);

        public int AbilityCount => _abilitiable.AbilityCount;

        public void Initialize(IReadOnlyList<IAbility> module) => _abilitiable.Initialize(module);

        public event EventHandler AbilitiesChanged
        {
            add => _abilitiable.AbilitiesChanged += value;
            remove => _abilitiable.AbilitiesChanged -= value;
        }

        public event EventHandler<SpellEventArgs> SpellCasted
        {
            add => _abilitiable.SpellCasted += value;
            remove => _abilitiable.SpellCasted -= value;
        }

        public void NotifySpellCast(SpellEventArgs e) => _abilitiable.NotifySpellCast(e);
    }
}