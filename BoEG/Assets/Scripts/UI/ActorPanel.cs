using MobaGame.Framework.Core;
using UnityEngine;

namespace MobaGame.UI
{
    public class ActorPanel : MonoBehaviour
    {
#pragma warning disable 0649
        [SerializeField] private bool _recollectUi;
        [SerializeField] private bool _setTarget;
        [SerializeField] private Actor _target;
        private DebugActorUI[] _uis;
#pragma warning restore 0649


        private void Awake()
        {
            _setTarget = false;
            _recollectUi = false;
            _uis = GetComponentsInChildren<DebugActorUI>();
        }

        private void Start()
        {
            SetInspectorTarget();
        }

        private void Update()
        {
            if (_recollectUi)
            {
                _uis = GetComponentsInChildren<DebugActorUI>();
                _recollectUi = false;
            }

            if (_setTarget)
            {
                SetInspectorTarget();
                _setTarget = false;
            }
        }

        private void SetInspectorTarget() => SetTarget(_target);

        public void SetTarget(Actor target)
        {
            foreach (var ui in _uis) ui.SetTarget(target);
        }

    }
}