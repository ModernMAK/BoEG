using UnityEngine;

namespace Triggers
{
    public class TriggerMethod
    {
        public TriggerMethod()
        {
            _follow = null;
            _position = default(Vector3);
            Mask = Physics.DefaultRaycastLayers;
        }


        public virtual Collider[] Collide()
        {
            return new Collider[0];
        }

        private Vector3 _position;
        private Transform _follow;

        protected Vector3 Position
        {
            get { return (_follow != null ? _follow.position : _position); }
        }

        protected int Mask { get; private set; }

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