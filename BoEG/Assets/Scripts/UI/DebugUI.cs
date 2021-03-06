using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace MobaGame.UI
{
    public abstract class DebugUI<TTarget> : MonoBehaviour
    {
        public abstract void SetTarget(TTarget target);

        private float FloorToPlace(float value, int place = 0)
        {
            var scale = Mathf.Pow(10f, place);
            return Mathf.FloorToInt(scale * value) / scale;
        }

        protected void UpdateText(float value, Text text, int place = 0, bool ignoreNull = false)
        {
            if (ignoreNull && text == null) return;
            var rounded = FloorToPlace(value, place);
            text.text = rounded.ToString(CultureInfo.InvariantCulture);
        }

        protected void UpdateImageFill(float value, Image img, int place = 0, bool ignoreNull = false)
        {
            if (ignoreNull && img == null) return;
            var rounded = FloorToPlace(value, place);
            var clamped = Mathf.Clamp01(rounded);
            img.fillAmount = clamped;
        }

        protected void UpdateSliderFill(float value, Slider slider, int place = 0, bool ignoreNull = false)
        {
            if (ignoreNull && slider == null) return;

            var rounded = FloorToPlace(value, place);
            var clamped = Mathf.Clamp01(rounded);
            slider.normalizedValue = clamped;
        }
    }
}