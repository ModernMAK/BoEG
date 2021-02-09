using System;
using UnityEngine;

namespace MobaGame.Framework.Core.Modules.Ability
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

        public virtual void Initialize(Actor data)
        {
            Self = data;
            AbilityHelper.Initialize(); //HACK TODO make this not a hack
            _cache = new ModuleCache(data.gameObject);
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


        ICooldownAbilityView IAbilityView.Cooldown => this as ICooldownAbilityView;

        IStatCostAbilityView IAbilityView.StatCost => this as IStatCostAbilityView;

        IToggleableAbilityView IAbilityView.Toggleable => this as IToggleableAbilityView;
    }
}