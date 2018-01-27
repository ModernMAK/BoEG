using Trigger;
using UnityEngine;

namespace Entity
{
    public class Visionable : UnetBehaviour
    {
        [SerializeField] private float _baseNormVisRange;

        [SerializeField] private float _baseTrueVisRange;

        private SphereTrigger _normTrigger;

        private SphereTrigger _trueTrigger;

        public float BaseNormVisRange
        {
            get { return _baseNormVisRange; }
            set
            {
                if (_baseNormVisRange != value)
                    SetDirtyBit(1);
                _baseNormVisRange = value;
            }
        }

        public virtual float NormVisRange
        {
            get { return BaseNormVisRange; }
        }

        public float BaseTrueVisRange
        {
            get { return _baseTrueVisRange; }
            set
            {
                if (_baseTrueVisRange != value)
                    SetDirtyBit(2);
                _baseTrueVisRange = value;
            }
        }

        public virtual float TrueVisRange
        {
            get { return BaseTrueVisRange; }
        }

        protected override void Awake()
        {
            base.Awake();

            _normTrigger = this.CreateTrigger<SphereTrigger>(null, "Vision");
            _trueTrigger = this.CreateTrigger<SphereTrigger>(new InvisVisionTriggerStrategy(this), "True Vision");
        }


        /*
    TRIGGERS (Of the top of my head, biggest uses are...)
        Aura's (Apply buff in Area)
        AOE's (Who's in/Who's out)

    */


        protected override void Update()
        {
            base.Update();
            _normTrigger.Radius = NormVisRange;
            _trueTrigger.Radius = TrueVisRange;
        }
    }
}