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


        private void Start()
        {
            _setTarget = true;
            _recollectUi = true;
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
    }
}