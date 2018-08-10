using UnityEngine;

namespace Triggers
{
    public class SphereTriggerMethod : TriggerMethod
    {
        
        public override Collider[] Collide()
        {
            return Physics.OverlapSphere(Position, _radius, Mask);
        }

        private float _radius;

        public SphereTriggerMethod SetRadius(float radius)
        {
            _radius = radius;
            return this;
        }
    }
}