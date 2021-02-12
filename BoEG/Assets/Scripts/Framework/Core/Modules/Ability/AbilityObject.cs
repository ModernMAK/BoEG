using System;
using UnityEngine;

namespace MobaGame.Framework.Core.Modules.Ability
{
    public class AbilityObject : ScriptableObject, IAbility
    {
#pragma warning disable 0649


        private ModuleCache _cache;
#pragma warning disable 0649

        protected ModuleCache Modules => _cache;

        public Actor Self { get; private set; }

        public virtual void Initialize(Actor data)
        {
            Self = data;
            AbilityHelper.Initialize(); //HACK TODO make this not a hack
            _cache = new ModuleCache(data.gameObject);
        }

        public virtual void Setup()
        {
            
        }


        /// <summary>
        ///     Used to ask the Ability to setup Ability Preview
        /// </summary>
        public virtual void SetupCast()
        {
        }

        /// <summary>
        ///     Used to confirm the Ability, if the ability cannot cast, the ability
        /// </summary>
        public virtual void ConfirmCast()
        {
        }

        /// <summary>
        ///     Used to reset the Ability Preview
        /// </summary>
        public virtual void CancelCast()
        {
        }

        public abstract IAbilityView GetAbilityView();
    }
}