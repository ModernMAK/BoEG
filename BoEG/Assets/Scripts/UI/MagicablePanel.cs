using Framework.Core;
using MobaGame.Framework.Core.Modules;
using UnityEngine;
using UnityEngine.UI;

namespace MobaGame.UI
{
    public class MagicablePanel : DebugUI
    {
        // Start is called before the first frame update
        private GameObject _go;
        private IMagicable _magicable;

        public override void SetTarget(GameObject go)
        {
            _go = go;
            _magicable = _go != null ? _go.GetModule<IMagicable>() : null;
        }

        // Update is called once per frame
        private void Update()
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