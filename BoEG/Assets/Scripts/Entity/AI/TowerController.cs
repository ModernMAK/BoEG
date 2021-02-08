using Framework.Core;
using MobaGame.Framework.Core;
using MobaGame.Framework.Core.Modules;
using UnityEngine;

namespace MobaGame.Entity.AI
{
    // TODO abstract this into a 'Controller' which can be used for AI or Players.
    public class TowerController : MonoBehaviour
    {
        private IAttackerable _attackerable;

        private void Start()
        {
            _attackerable = this.GetComponent<Actor>().GetModule<IAttackerable>();
            enabled = (_attackerable != null);
        }

        // Update is called once per frame
        void Update()
        {
            if (_attackerable.HasTarget() && !_attackerable.OnCooldown)
                _attackerable.Attack(_attackerable.Targets[0]);
        }
    }
}