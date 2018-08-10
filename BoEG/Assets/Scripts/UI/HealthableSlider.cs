using Modules.Healthable;

namespace UI
{
    public class HealthableSlider : SliderUtil
    {
        private IHealthable _healthable;
        protected override void Awake()
        {
            _healthable = _target.GetComponent<IHealthable>();
            base.Awake();
        }

        protected override float NormalizedValue
        {
            get { return _healthable.HealthPercentage; }
        }
    }
}