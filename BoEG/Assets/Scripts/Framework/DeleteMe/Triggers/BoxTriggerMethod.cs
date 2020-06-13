using UnityEngine;

namespace Triggers
{
    public class BoxTriggerMethod : TriggerMethod
    {
        private Quaternion _rotation;

        private Vector3 _scale;

        public override Collider[] Collide()
        {
            return Physics.OverlapBox(Position, _scale, _rotation, Mask);
        }

        public BoxTriggerMethod SetRadius(Vector3 scale)
        {
            _scale = scale;
            return this;
        }

        public BoxTriggerMethod SetRadius(Quaternion rotation)
        {
            _rotation = rotation;
            return this;
        }
    }
}