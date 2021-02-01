using Framework.Core;
using MobaGame.Framework.Core;
using MobaGame.Framework.Core.Modules;
using MobaGame.UI;
using UnityEngine;

namespace MobaGame
{
    public class AttackRangeDebugFX : DebugActorUI
    {
        private IAttackerable _attackerable;

        // Start is called before the first frame update
        private Actor _go;

        public override void SetTarget(Actor go)
        {
            _go = go;
            _attackerable = _go != null ? _go.GetModule<IAttackerable>() : null;
        }

        // Update is called once per frame
        private void Update()
        {
            var attackRange = 0f;    
            if (_attackerable != null)
            {
                attackRange = _attackerable.Range;
            }

            _renderer.SetRadius(attackRange, true);
        }

#pragma warning disable 649
        [SerializeField] private RingRenderer _renderer;
#pragma warning restore 649

        void Awake()
        {
            if (_renderer == null)
                _renderer = GetComponent<RingRenderer>();
        }
    }
}