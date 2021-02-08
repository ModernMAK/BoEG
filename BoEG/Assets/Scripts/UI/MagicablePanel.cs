using System;
using MobaGame.Framework.Core;
using MobaGame.Framework.Core.Modules;
using UnityEngine;
using UnityEngine.UI;

namespace MobaGame.UI
{
    public class MagicablePanel : DebugModuleUI<IMagicable>
    {
        // Start is called before the first frame update
        private IMagicable _magicable;
        private IMagicableView View => _magicable.View;
        private bool IsNull => _magicable == null || View == null;
        public override void SetTarget(IMagicable target)
        {
            if (!IsNull)
                View.Changed -= View_Changed;
            _magicable = target;
            UpdateUI();
            if (!IsNull)
                View.Changed += View_Changed;
        }

        private void View_Changed(object sender, EventArgs e) => UpdateUI();

        // Update is called once per frame
        private void UpdateUI()
        {
            var value = 0f;
            var generation = 0f;
            var capacity = 0f;
            var normal = 0f;
            if (_magicable != null)
            {
                value = _magicable.Value;
                capacity = _magicable.Capacity.Total;
                generation = _magicable.Generation.Total;
                normal = _magicable.Percentage;
            }

            UpdateText(value, _magicValueText, 1);
            UpdateText(generation, _magicGenerationText, 1);
            UpdateText(capacity, _magicCapacityText, 1);
            UpdateImageFill(normal, _magicNormalImage, 3);
        }
#pragma warning disable 649
        [SerializeField] private Text _magicValueText;
        [SerializeField] private Text _magicCapacityText;
        [SerializeField] private Text _magicGenerationText;
        [SerializeField] private Image _magicNormalImage;
#pragma warning restore 649
    }
}