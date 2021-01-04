using System;
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
        private IHealthable _healthable;

        private void Awake()
        {
            _healthable = GetComponent<IHealthable>();
            _healthable.Died += HealthableOnDied;
        }

        private void HealthableOnDied(object sender, EventArgs e)
        {
            switch (deathType)
            {
                case DeathType.None:
                    break;
                case DeathType.Destroy:
                    Destroy(gameObject);
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