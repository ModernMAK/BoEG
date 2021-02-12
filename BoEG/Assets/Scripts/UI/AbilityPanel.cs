using System;
using MobaGame.Framework.Core.Modules.Ability;
using UnityEngine;
using UnityEngine.UI;

namespace MobaGame.UI
{
    public class AbilityPanel : DebugUI<IAbility>
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
            
            _iconCooldown.sprite = _icon.sprite = _view.Icon;
            
            UpdateImageFill(0f, _iconCooldown);
            if (_view.Cooldown != null)
                UpdateImageFill(1f - _view.Cooldown.Normal, _iconCooldown, 3);

            bool outOfMana = false;
            bool isActive = false;
            bool showActive = true;
            if (_view.StatCost != null)
            {
                outOfMana = !_view.StatCost.CanSpendCost();
            }

            if (_view.Toggleable != null)
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


        private bool _updateCooldown;
        [SerializeField] private Image _iconCooldown;

        private bool _updateManaCost;
        [SerializeField] private Material _outOfManaMaterial;

        private bool _updateActive;
        [SerializeField] private Material _activeMaterial;

#pragma warning restore 649
    }
}