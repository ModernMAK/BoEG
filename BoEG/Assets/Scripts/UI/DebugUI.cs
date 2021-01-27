using MobaGame.Framework.Core;
using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace MobaGame.UI
{
    public static class ExceptionX
	{
        private static string CreateMissingComponentExceptionMessage(string gameObjectName, string componentName) => $"{gameObjectName} is missing component {componentName}!";
        public static MissingComponentException CreateMissingComponentException(string gameObjectName, string componentName, Exception innerException) => new MissingComponentException(CreateMissingComponentExceptionMessage(gameObjectName, componentName), innerException);
        public static MissingComponentException CreateMissingComponentException(string gameObjectName, string componentName) => new MissingComponentException(CreateMissingComponentExceptionMessage(gameObjectName, componentName));
    }
    public abstract class DebugActorUI : DebugUI
    {
		public override void SetTarget(GameObject go)
		{
            if (go != null)
                if (go.TryGetComponent<Actor>(out var actor))
                    SetTarget(actor);
                else
                    throw ExceptionX.CreateMissingComponentException(go.name, nameof(Actor));
            else
                SetTarget(null);
		}
        public abstract void SetTarget(Actor actor);
	}
    public abstract class DebugUI : MonoBehaviour
    {
        public abstract void SetTarget(GameObject go);

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