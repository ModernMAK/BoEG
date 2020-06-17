using UI;
using UnityEngine;

namespace System
{
    public class ActorPanel : MonoBehaviour
    {
        [SerializeField] private bool _recollectUi;
        [SerializeField] private bool _setTarget;
        [SerializeField] private GameObject _target;
        private DebugUI[] _uis;


        private void Awake()
        {
            _setTarget = false;
            _recollectUi = false;
            _uis = GetComponentsInChildren<DebugUI>();
            SetTargets();
        }

        private void Update()
        {
            if (_recollectUi)
            {
                _uis = GetComponentsInChildren<DebugUI>();
                _recollectUi = false;
            }

            if (_setTarget)
            {
                SetTargets();
                _setTarget = false;
            }
        }

        private void SetTargets()
        {
            foreach (var ui in _uis) ui.SetTarget(_target);
        }

        public void SetTarget(GameObject target)
        {
            foreach (var ui in _uis) ui.SetTarget(target);
        }
    }
}