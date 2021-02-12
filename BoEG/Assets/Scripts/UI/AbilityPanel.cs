using System;
using MobaGame.Framework.Core.Modules.Ability;
using UnityEngine;
using UnityEngine.UI;

namespace MobaGame.UI
{
    public class AbilityPanel : DebugUI<IAbility>, IDisposable
    {
        private IAbilityView _view;

        public override void SetTarget(IAbility ability)
        {
            if(_view != null)
                _view.Changed -= ViewOnChanged;
            _view = ability.GetAbilityView();
            UpdateMain();
            if(_view != null)
                _view.Changed += ViewOnChanged;
        }

        private void ViewOnChanged(object sender, EventArgs e)
        {
            UpdateMain();
        }

        private void UpdateMain()
        {
            var icon = _view != null ? _view.Icon : null;
            _iconCooldown.sprite = _icon.sprite = icon;
            UpdateImageFill(0f, _iconCooldown);

            var cdNormal = 1f;
            if (_view != null && _view.Cooldown != null)
                cdNormal = _view.Cooldown.Normal;
            UpdateImageFill(1f - cdNormal, _iconCooldown, 3);

            bool outOfMana = false;
            bool isActive = false;
            bool showActive = true;
            if (_view != null && _view.StatCost != null)
            {
                outOfMana = !_view.StatCost.CanSpendCost();
            }

            if (_view != null && _view.Toggleable != null)
            {
                showActive = _view.Toggleable.ShowActive;
                isActive = _view.Toggleable.Active;
            }

            if (showActive && isActive)
                _icon.material = _activeMaterial;
            else if (outOfMana)
                _icon.material = _outOfManaMaterial;
            else
                _icon.material = _standardMaterial;
        }

        private void Awake()
        {
            _standardMaterial = _icon.material;
        }

#pragma warning disable 649
        [SerializeField] private Image _icon;
        private Material _standardMaterial;


        [SerializeField] private Image _iconCooldown;

        [SerializeField] private Material _outOfManaMaterial;

        [SerializeField] private Material _activeMaterial;

#pragma warning restore 649
        public void Dispose()
        {
            if(_view != null)
                _view.Changed -= ViewOnChanged;
        }
    }
}