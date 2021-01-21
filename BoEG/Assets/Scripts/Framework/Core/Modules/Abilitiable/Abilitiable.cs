using System;
using System.Collections.Generic;
using MobaGame.Framework.Core.Modules.Ability;

namespace MobaGame.Framework.Core.Modules
{
    public class Abilitiable : ActorModule, IAbilitiable, IInitializable<IReadOnlyList<IAbility>>, IRespawnable
    {
        private IAbility[] _abilities;
        private IReadOnlyList<IRespawnable> _respawnables;

        public Abilitiable(Actor actor) : base(actor)
        {
            _abilities = new IAbility[0];
        }

        public Abilitiable(Actor actor, IReadOnlyList<IAbility> abilities) : this(actor)
        {
            _abilities = new IAbility[abilities.Count];
            Initialize(abilities);
        }


        public void Initialize(IReadOnlyList<IAbility> abilities)
        {
            _abilities = new IAbility[abilities.Count];
            for (var i = 0; i < _abilities.Length; i++)
                _abilities[i] = abilities[i];

            foreach (var ab in _abilities)
                ab.Initialize(Actor);
            _respawnables = EnumerableQuery.GetAllAsList<IRespawnable>(_abilities);
            OnAbilitiesChanged();
        }

        public bool TryGetAbility<T>(out T ability) => EnumerableQuery.TryGet<T>(_abilities, out ability);

        public T GetAbility<T>() => EnumerableQuery.Get<T>(_abilities);

        public IAbility GetAbility(int index)
        {
            return _abilities[index];
        }

        public int AbilityCount => _abilities.Length;

        public event EventHandler AbilitiesChanged
        {
            add => _abilitiesChanged += value;
            remove => _abilitiesChanged -= value;
        }

        private event EventHandler _abilitiesChanged;

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

        protected virtual void OnAbilitiesChanged()
        {
            _abilitiesChanged?.Invoke(this, EventArgs.Empty);
        }

        public void Respawn()
        {
            foreach (var respawnable in _respawnables)
            {
                respawnable.Respawn();
            }
        }
    }
}