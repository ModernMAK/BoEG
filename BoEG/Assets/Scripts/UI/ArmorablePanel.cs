using System;
using MobaGame.Framework.Core;
using MobaGame.Framework.Core.Modules;
using UnityEngine;
using UnityEngine.UI;

namespace MobaGame.UI
{
    public class ArmorablePanel : DebugModuleUI<IArmorable>
    {
        private IArmorable _armorable;
        private IArmorableView View => _armorable.View;

        private bool IsNull => _armorable == null || View == null;

        public override void SetTarget(IArmorable target)
        {
            if(!IsNull)
                View.Changed -= ViewOnChanged;
            _armorable = target;
            UpdateUI();
            if(!IsNull)
                View.Changed += ViewOnChanged;
        }

        private void ViewOnChanged(object sender, EventArgs e) => UpdateUI();

        // Update is called once per frame
        private void UpdateUI()
        {
            var physBlock = 0f;
            var magBlock = 0f;
            var physResist = 0f;
            var magResist = 0f;
            if (_armorable != null)
            {
                physBlock = _armorable.Physical.Block;
                physResist = _armorable.Physical.Resistance * 100f;
                magBlock = _armorable.Magical.Block;
                magResist = _armorable.Magical.Resistance * 100f;
            }

            UpdateText(physBlock, _physicalBlockText);
            UpdateText(magBlock, _magicalBlockText);
            UpdateText(physResist, _physicalResistText);
            UpdateText(magResist, _magicalResistText);
        }

#pragma warning disable 649
        [SerializeField] private Text _physicalBlockText;
        [SerializeField] private Text _physicalResistText;
        [SerializeField] private Text _magicalBlockText;

        [SerializeField] private Text _magicalResistText;
#pragma warning restore 649
    }
}