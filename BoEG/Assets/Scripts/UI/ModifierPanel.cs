using System;
using MobaGame.Framework.Core;
using UnityEngine;
using UnityEngine.UI;

namespace MobaGame.UI
{
    public class ModifierPanel : DebugUI<IModifier>
    {
        private IModifier _modifier;
        private IModifierView View => _modifier.View;

        public override void SetTarget(IModifier target)
        {
            if (_modifier != null && View != null)
                View.Changed -= UpdateUI;
            _modifier = target;
            UpdateUI();
            if (_modifier != null && View != null)
                View.Changed += UpdateUI;
        }

        private void UpdateUI()
        {
            
            if (_icon != null)
                _icon.sprite = View.Icon;
        }

        private void UpdateUI(object sender, EventArgs args) => UpdateUI();

#pragma warning disable 649
        [SerializeField] private Image _icon;
#pragma warning restore 649
    }
}