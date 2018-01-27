using UnityEngine;
using UnityEngine.Networking;

namespace Trigger
{
    [RequireComponent(typeof(SphereCollider))]
    public class SphereTrigger : Trigger
    {
        [SerializeField] private float _radius;

        public float Radius
        {
            get { return _radius; }
            set
            {
                if (value != _radius)
                    SetDirtyBit(1);
                _radius = value;
            }
        }

        public SphereCollider Collider { get; private set; }

        protected override void Awake()
        {
            Collider = GetComponent<SphereCollider>();
        }

        protected override void Update()
        {
            base.Update();
            Collider.radius = Radius;
        }

        protected override void ModuleSerialize(ModuleWriter writer)
        {
            base.ModuleSerialize(writer);
            writer.WriteIf(Radius, 1);
        }

        protected override void ModuleDeserialize(NetworkReader reader, uint mask, bool readMask = false)
        {
            base.ModuleDeserialize(reader, mask);
        }
    }
}