using MobaGame.Framework.Core;
using UnityEngine;
using UnityEngine.UI;

namespace MobaGame.UI
{
    public class IconPanel : DebugActorUI
    {
#pragma warning disable 0649

        [SerializeField] private Image _icon;
#pragma warning restore 0649

        public override void SetTarget(Actor target)
        {
            _actor = target;
        }

        private void Update()
        {
            if (_icon != null)
            {
                _icon.sprite = _actor != null ? _actor.GetIcon() : null;
            }
        }


        private Actor _actor;
    }
}