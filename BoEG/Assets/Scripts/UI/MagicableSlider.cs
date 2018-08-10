using Modules.Magicable;

namespace UI
{
    public class MagicableSlider : SliderUtil
    {
        private IMagicable _magicable;
        protected override void Awake()
        {
            _magicable = _target.GetComponent<IMagicable>();
            base.Awake();
        }

        protected override float NormalizedValue
        {
            get { return _magicable.ManaPercentage; }
        }
    }
}