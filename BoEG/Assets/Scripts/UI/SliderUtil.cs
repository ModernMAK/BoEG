using UnityEngine;

namespace UI
{
    public class SliderUtil : MonoBehaviour
    {
        public GameObject _target;
        private UnityEngine.UI.Slider _slider;

        protected virtual void Awake()
        {
            _slider = GetComponent<UnityEngine.UI.Slider>();
        }
        protected virtual float NormalizedValue
        {
            get { return 0f; }
        }

        private void Update()
        {
            _slider.normalizedValue = NormalizedValue;
        }
    
    }
}