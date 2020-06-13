using UnityEngine;

namespace Triggers
{
    public class TriggerMethod
    {
        private Transform _follow;

        private Vector3 _position;

        public TriggerMethod()
        {
            _follow = null;
            _position = default;
            Mask = Physics.DefaultRaycastLayers;
        }

        protected Vector3 Position => _follow != null ? _follow.position : _position;

        protected int Mask { get; private set; }


        public virtual Collider[] Collide()
        {
            return new Collider[0];
        }

        public TriggerMethod SetFollow(GameObject go)
        {
            return SetFollow(go.transform);
        }

        public TriggerMethod SetFollow(Transform trans)
        {
            _follow = trans;
            return this;
        }

        public TriggerMethod SetPosition(Vector3 position)
        {
            _position = position;
            _follow = null;
            return this;
        }

        public TriggerMethod SetLayerMask(int mask = Physics.DefaultRaycastLayers)
        {
            Mask = mask;
            return this;
        }
    }
}