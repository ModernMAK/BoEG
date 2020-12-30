using Framework.Core.Modules;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HealthablePanel : DebugUI
    {
        // Start is called before the first frame update
        private GameObject _go;
        private IHealthable _healthable;

        public override void SetTarget(GameObject go)
        {
            _go = go;
            _healthable = _go != null ? _go.GetComponent<IHealthable>() : null;
        }

        // Update is called once per frame
        private void Update()
        {
            var value = 0f;
            var generation = 0f;
            var capacity = 0f;
            var normal = 0f;
            if (_healthable != null)
            {
                value = _healthable.Health;
                capacity = _healthable.HealthCapacity;
                generation = _healthable.HealthGeneration;
                normal = _healthable.HealthPercentage;
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