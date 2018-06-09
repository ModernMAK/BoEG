using Components.Healthable;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class HealthableSlider : MonoBehaviour
{
    private Slider _slider;
    private IHealthable _healthable;

    private void Awake()
    {
        _healthable = transform.root.GetComponent<IHealthable>();
        _slider = GetComponent<Slider>();
    }

    private void Update()
    {
        if (_slider == null)
            return;
        if (_healthable == null)
            return;

        _slider.normalizedValue = _healthable.HealthRatio;
    }
}