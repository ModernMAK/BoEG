using UnityEngine;

namespace Triggers
{
    public class SphereTriggerMethod : TriggerMethod
    {
        private float _radius;

        public override Collider[] Collide()
        {
            return Physics.OverlapSphere(Position, _radius, Mask);
        }

        public SphereTriggerMethod SetRadius(float radius)
        {
            _radius = radius;
            return this;
        }
    }
}