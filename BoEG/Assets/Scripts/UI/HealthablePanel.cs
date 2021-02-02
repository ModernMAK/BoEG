using MobaGame.Framework.Core;
using MobaGame.Framework.Core.Modules;
using UnityEngine;
using UnityEngine.UI;

namespace MobaGame.UI
{
    public class HealthablePanel : DebugModuleUI<IHealthable>
    {
        private IHealthable _healthable;
        private IHealthableView View => _healthable.View;
        private bool IsNull => _healthable == null || View == null;
        public override void SetTarget(IHealthable target)
        {
            if (!IsNull)
                View.Changed -= View_Changed;
            _healthable = target;
            UpdateUI();
            if (!IsNull)
                View.Changed += View_Changed;
        }

        private void View_Changed(object sender, System.EventArgs e) => UpdateUI();

		// Update is called once per frame
		private void UpdateUI()
        {
            var value = 0f;
            var normal = 0f;
            var generation = 0f;
            var capacity = 0f;
            if (_healthable != null)
            {
                value = _healthable.Value;               
                generation = _healthable.Generation.Total;
                capacity = _healthable.Capacity.Total;            
                normal = _healthable.Percentage;
            }
            UpdateText(value, _healthValueText, 1);
            UpdateText(generation, _healthGenerationText, 1);
            UpdateText(capacity, _healthCapacityText, 1);      
            UpdateImageFill(normal, _healthNormalImage, 3);
        }
#pragma warning disable 649
        [SerializeField] private Text _healthValueText;
        [SerializeField] private Text _healthCapacityText;
        [SerializeField] private Text _healthGenerationText;
        [SerializeField] private Image _healthNormalImage;
#pragma warning restore 649
    }
}