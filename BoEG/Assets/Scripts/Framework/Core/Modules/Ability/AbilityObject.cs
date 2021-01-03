using System;
using Entity.Abilities.FlameWitch;
using Framework.Core;
using Framework.Core.Modules;
using Modules.Teamable;
using UnityEngine;

namespace Framework.Ability
{
    public class AbilityObject : ScriptableObject, IAbility, IAbilityView
    {
#pragma warning disable 0649

        [SerializeField] private Sprite _icon;
        [Obsolete("Use Actor Module Cache")] protected CommonAbilityInfo _commonAbilityInfo;

        private ModuleCache _cache;
#pragma warning disable 0649

        protected ModuleCache Modules => _cache;

        public Actor Self { get; private set; }
        protected bool IsSelf(GameObject gameObject) => gameObject == Self.gameObject;
        protected bool IsSelf(Actor actor) => actor == Self;

        public virtual void Initialize(Actor actor)
        {
            Self = actor;
            AbilityHelper.Initialize(); //HACK TODO make this not a hack
            _cache = new ModuleCache(actor.gameObject);
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

        public virtual IAbilityView GetAbilityView()
        {
            return this;
        }

        public virtual Sprite GetIcon()
        {
            return _icon;
        }


        ICooldownAbility IAbilityView.Cooldown => this as ICooldownAbility;

        IStatCostAbility IAbilityView.StatCost => this as IStatCostAbility;

        IToggleableAbility IAbilityView.Toggleable => this as IToggleableAbility;
    }
}