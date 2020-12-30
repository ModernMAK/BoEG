using System;
using UnityEngine;

namespace Framework.Core.Modules
{
    [RequireComponent(typeof(Collider))]
    public class Trigger : MonoBehaviour
    {
        private Collider _collider;
        private void Awake()
        {
            _collider = GetComponent<Collider>();
            _collider.isTrigger = true;
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