using System;
using UnityEngine;

namespace Framework.Core.Modules
{
    [RequireComponent(typeof(Collider))]
    public class Trigger : MonoBehaviour
    {
        private enum PhysicsShape
        {
            Invalid = -1,
            Box,
            Sphere,
            Capsule,
        }

        private PhysicsShape _shape;
        private Collider _collider;
        private BoxCollider _boxCollider;
        private SphereCollider _sphereCollider;
        private CapsuleCollider _capsuleCollider;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
            _collider.isTrigger = true;
            _boxCollider = null;
            _sphereCollider = null;
            _capsuleCollider = null;
            switch (_collider)
            {
                case BoxCollider box:
                    _boxCollider = box;
                    _shape = PhysicsShape.Box;
                    break;
                case SphereCollider sphere:
                    _sphereCollider = sphere;
                    _shape = PhysicsShape.Sphere;
                    break;
                case CapsuleCollider capsule:
                    _capsuleCollider = capsule;
                    _shape = PhysicsShape.Capsule;
                    break;
                default:
                    _shape = PhysicsShape.Invalid;
                    break;
            }
        }


        public Collider[] OverlapCollider()
        {
            return _shape switch
            {
                PhysicsShape.Sphere => _sphereCollider.OverlapSphere(),
                PhysicsShape.Box => _boxCollider.OverlapBox(),
                PhysicsShape.Capsule => _capsuleCollider.OverlapCapsule(),
                PhysicsShape.Invalid => throw new NotSupportedException(),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public Collider[] OverlapCollider(int layerMask)
        {
            return _shape switch
            {
                PhysicsShape.Sphere => _sphereCollider.OverlapSphere(layerMask),
                PhysicsShape.Box => _boxCollider.OverlapBox(layerMask),
                PhysicsShape.Capsule => _capsuleCollider.OverlapCapsule(layerMask),
                PhysicsShape.Invalid => throw new NotSupportedException(),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
        public Collider[] OverlapCollider(int layerMask, QueryTriggerInteraction query)
        {
            return _shape switch
            {
                PhysicsShape.Sphere => _sphereCollider.OverlapSphere(layerMask, query),
                PhysicsShape.Box => _boxCollider.OverlapBox(layerMask, query),
                PhysicsShape.Capsule => _capsuleCollider.OverlapCapsule(layerMask, query),
                PhysicsShape.Invalid => throw new NotSupportedException(),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private event EventHandler<TriggerEventArgs> _enter;
        private event EventHandler<TriggerEventArgs> _stay;
        private event EventHandler<TriggerEventArgs> _exit;

        public event EventHandler<TriggerEventArgs> Enter
        {
            add => _enter += value;
            remove => _enter -= value;
        }

        public event EventHandler<TriggerEventArgs> Stay
        {
            add => _stay += value;
            remove => _stay -= value;
        }

        public event EventHandler<TriggerEventArgs> Exit
        {
            add => _exit += value;
            remove => _exit -= value;
        }

        private void OnTriggerEnter(Collider other)
        {
            _enter?.Invoke(this, new TriggerEventArgs() {Collider = other});
        }

        private void OnTriggerExit(Collider other)
        {
            _exit?.Invoke(this, new TriggerEventArgs() {Collider = other});
        }

        private void OnTriggerStay(Collider other)
        {
            _stay?.Invoke(this, new TriggerEventArgs() {Collider = other});
        }
    }
}