using Components.Magicable;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class MagicableSlider : MonoBehaviour
{
    private Slider _slider;
    private IMagicable _healthable;

    private void Awake()
    {
        _healthable = transform.root.GetComponent<IMagicable>();
        _slider = GetComponent<Slider>();
    }

    private void Update()
    {
        if (_slider == null)
            return;
        if (_healthable == null)
            return;

        _slider.normalizedValue = _healthable.ManaRatio;
    }
}