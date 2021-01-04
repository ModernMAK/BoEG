using UnityEngine;

namespace MobaGame.UI
{
    public class ActorPanel : MonoBehaviour
    {
#pragma warning disable 0649
        [SerializeField] private bool _recollectUi;
        [SerializeField] private bool _setTarget;
        [SerializeField] private GameObject _target;
        private DebugUI[] _uis;
#pragma warning restore 0649


        private void Awake()
        {
            _setTarget = false;
            _recollectUi = false;
            _uis = GetComponentsInChildren<DebugUI>();
        }

        private void Start()
        {
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