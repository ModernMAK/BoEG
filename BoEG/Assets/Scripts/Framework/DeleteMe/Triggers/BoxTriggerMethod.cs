using UnityEngine;

namespace Triggers
{
    public class BoxTriggerMethod : TriggerMethod
    {
        public override Collider[] Collide()
        {
            return Physics.OverlapBox(Position, _scale, _rotation, Mask);
        }

        private Vector3 _scale;

        public BoxTriggerMethod SetRadius(Vector3 scale)
        {
            _scale = scale;
            return this;
        }
        private Quaternion _rotation;

        public BoxTriggerMethod SetRadius(Quaternion rotation)
        {
            _rotation = rotation;
            return this;
        }
    }
}