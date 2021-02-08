using System;
using Framework.Core;
using MobaGame.Framework.Core;
using MobaGame.Framework.Core.Modules;
using UnityEngine;

namespace MobaGame
{
    public class DeathLogic : MonoBehaviour
    {
        public enum DeathType
        {
            None,
            Destroy,
            Disable
        }

        public DeathType deathType;
        private IKillable _killable;
        private bool _markDestroy;

        private void Start()
        {
            if(this.TryGetComponent<Actor>(out var actor))
                if(actor.TryGetModule(out _killable))
                    _killable.Died += HealthableOnDied;
        }
		private void LateUpdate()
        {
            if(_markDestroy)
                Destroy(gameObject);
        }

		private void HealthableOnDied(object sender, EventArgs e)
        {
            switch (deathType)
            {
                case DeathType.None:
                    break;
                case DeathType.Destroy:
                    _markDestroy = true;
                    gameObject.SetActive(false);                       
                    break;
                case DeathType.Disable:
                    gameObject.SetActive(false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}