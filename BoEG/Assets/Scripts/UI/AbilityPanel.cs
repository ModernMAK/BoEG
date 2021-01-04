using MobaGame.Framework.Core.Modules.Ability;
using UnityEngine;
using UnityEngine.UI;

namespace MobaGame.UI
{
    public class AbilityPanel : DebugUI
    {
        private IAbilityView _view;

        public override void SetTarget(GameObject go)
        {
        }


        public void SetAbility(IAbility ability)
        {
            _view = ability.GetAbilityView();
            _iconCooldown.sprite = _icon.sprite = _view.GetIcon();
            _updateCooldown = _view.Cooldown != null;
            _updateManaCost = _view.StatCost != null;
            _updateActive = _view.Toggleable != null;
            UpdateImageFill(0f, _iconCooldown);
        }

        private void Update()
        {
            if (_updateCooldown)
                UpdateImageFill(1f - _view.Cooldown.CooldownNormal, _iconCooldown, 3);

            bool outOfMana = false;
            bool isActive = false;
            if (_updateManaCost)
            {
                outOfMana = !_view.StatCost.CanSpendCost();
            }

            if (_updateActive)
            {
                isActive = _view.Toggleable.Active;
            }

            if (isActive)
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