using MobaGame.Framework.Core.Modules.Ability;
using System;
using System.Collections.Generic;

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

        /// <summary>
        /// Event fired when the Abilities list changes.
        /// </summary>
        /// <remarks>
        /// This does not fire when the contents of the list changes.
        /// </remarks>
        public event EventHandler AbilitiesChanged;

        /// <summary>
        /// Event fired when an ability notifies us of an ability cast.
        /// </summary>
        public event EventHandler<AbilityEventArgs> AbilityCasted;


        protected virtual void OnAbilityCasted(AbilityEventArgs e)
        {
            AbilityCasted?.Invoke(this, e);
        }

        protected virtual void OnAbilitiesChanged()
        {
            AbilitiesChanged?.Invoke(this, EventArgs.Empty);
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
            foreach (var ab in _abilities)
                ab.Setup();
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

        public bool TryGetAbility<TAbility>(out TAbility ability) where TAbility : IAbility => EnumerableQuery.TryGet(_abilities, out ability);
        

        public T GetAbility<T>() => EnumerableQuery.Get<T>(_abilities);
        

        public IReadOnlyList<IAbility> Abilities => _abilities;


        public void NotifyAbilityCast(AbilityEventArgs e) =>             OnAbilityCasted(e);
        

        #endregion
    }
}