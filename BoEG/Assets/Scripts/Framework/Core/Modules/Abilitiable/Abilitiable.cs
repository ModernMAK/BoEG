using System;
using System.Collections.Generic;
using MobaGame.Framework.Core.Modules.Ability;

namespace MobaGame.Framework.Core.Modules
{
    public class Abilitiable : ActorModule, IAbilitiable, IInitializable<IReadOnlyList<IAbility>>, IRespawnable
    {
        #region Variables

        /// <summary>
        /// The abilities belonging to this actor.
        /// </summary>
        private readonly List<IAbility> _abilities;

        /// <summary>
        /// A cache of the respawnable abilities.
        /// </summary>
        private IReadOnlyList<IRespawnable> _respawnables;


        #endregion

        #region Properties

        public int AbilityCount => _abilities.Count;

        #endregion

        #region Constructors

        public Abilitiable(Actor actor) : base(actor)
        {
            _abilities = new List<IAbility>();
        }

        public Abilitiable(Actor actor, IReadOnlyList<IAbility> abilities) : this(actor)
        {
            Initialize(abilities);
        }

        #endregion

        #region Events

        private event EventHandler _abilitiesChanged;

        private event EventHandler<SpellEventArgs> _spellCasted;

        public event EventHandler AbilitiesChanged
        {
            add => _abilitiesChanged += value;
            remove => _abilitiesChanged -= value;
        }


        public event EventHandler<SpellEventArgs> AbilityCasted
        {
            add => _spellCasted += value;
            remove => _spellCasted -= value;
        }


        protected virtual void OnSpellCast(SpellEventArgs e)
        {
            _spellCasted?.Invoke(this, e);
        }

        protected virtual void OnAbilitiesChanged()
        {
            _abilitiesChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        

        #region IInitializable

        public void Initialize(IReadOnlyList<IAbility> abilities)
        {
            _abilities.Clear();
            _abilities.AddRange(abilities);
            foreach (var ab in _abilities)
                ab.Initialize(Actor);
            _respawnables = EnumerableQuery.GetAllAsList<IRespawnable>(_abilities);
            OnAbilitiesChanged();
        }

        #endregion

        #region IRespawnable

        public void Respawn()
        {
            foreach (var respawnable in _respawnables) respawnable.Respawn();
        }

        #endregion

        #region IAbilitiable

        public bool TryGetAbility<TAbility>(out TAbility ability) where TAbility : IAbility
        {
            return EnumerableQuery.TryGet(_abilities, out ability);
        }

        public T GetAbility<T>()
        {
            return EnumerableQuery.Get<T>(_abilities);
        }


        public IAbility GetAbility(int index)
        {
            return _abilities[index];
        }

        public IReadOnlyList<IAbility> Abilities => _abilities;


        public void NotifySpellCast(SpellEventArgs e)
        {
            OnSpellCast(e);
        }

        #endregion
    }
}