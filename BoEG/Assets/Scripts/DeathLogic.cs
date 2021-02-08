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

        private void Start()
        {
            if(this.TryGetComponent<Actor>(out var actor))
                if(actor.TryGetModule(out _killable))
                    _killable.Died += HealthableOnDied;
        }

		private void HealthableOnDied(object sender, EventArgs e)
        {
            switch (deathType)
            {
                case DeathType.None:
                    break;
                case DeathType.Destroy:
                    gameObject.SetActive(false);                       
                    Destroy(gameObject,1f);
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