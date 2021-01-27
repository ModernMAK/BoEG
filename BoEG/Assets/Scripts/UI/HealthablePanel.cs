using Framework.Core;
using MobaGame.Framework.Core.Modules;
using UnityEngine;
using UnityEngine.UI;

namespace MobaGame.UI
{
    public class HealthablePanel : DebugUI
    {
        // Start is called before the first frame update
        private GameObject _go;
        private IHealthable _healthable;

        public override void SetTarget(GameObject go)
        {
            _go = go;
            _healthable = _go != null ? _go.GetModule<IHealthable>() : null;
        }

        // Update is called once per frame
        private void Update()
        {
            var value = 0f;
            var normal = 0f;
            var baseGeneration = 0f;
            var bonusGeneration = 0f;
            var baseCapacity = 0f;
            var bonusCapacity = 0f;
            if (_healthable != null)
            {
                value = _healthable.Health;
                
                baseGeneration = _healthable.BaseHealthGeneration;
                bonusGeneration = _healthable.BonusHealthGeneration;

                baseCapacity = _healthable.BaseHealthCapacity;
                bonusCapacity = _healthable.BonusHealthCapacity;
                
                normal = _healthable.HealthPercentage;
            }

            UpdateText(value, _healthValueText, 1);
            UpdateText(baseGeneration, _healthBaseGenerationText, 1);
            UpdateText(bonusGeneration, _healthBonusGenerationText, 1);
            UpdateText(baseCapacity, _healthBaseCapacityText, 1);
            UpdateText(bonusCapacity, _healthBonusCapacityText, 1);
            UpdateImageFill(normal, _healthNormalImage, 3);
        }
#pragma warning disable 649
        [SerializeField] private Text _healthValueText;
        [SerializeField] private Text _healthBaseCapacityText;
        [SerializeField] private Text _healthBonusCapacityText;
        [SerializeField] private Text _healthBaseGenerationText;
        [SerializeField] private Text _healthBonusGenerationText;
        [SerializeField] private Image _healthNormalImage;
#pragma warning restore 649
    }
}