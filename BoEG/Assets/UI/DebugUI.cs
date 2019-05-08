using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public abstract class DebugUI : MonoBehaviour
    {
        public abstract void SetTarget(GameObject go);

        private float FloorToPlace(float value, int place = 0)
        {
            var scale = Mathf.Pow(10f, place);
            return Mathf.FloorToInt(scale * value) / scale;
        }

        protected void UpdateText(float value, Text text, int place = 0)
        {
            var rounded = FloorToPlace(value, place);
            text.text = rounded.ToString(CultureInfo.InvariantCulture);
        }

        protected void UpdateImageFill(float value, Image img, int place = 0)
        {
            var rounded = FloorToPlace(value, place);
            var clamped = Mathf.Clamp01(rounded);
            img.fillAmount = clamped;
        }

        protected void UpdateSliderFill(float value, Slider slider, int place = 0)
        {
            var rounded = FloorToPlace(value, place);
            var clamped = Mathf.Clamp01(rounded);
            slider.normalizedValue = clamped;
        }
    }
}