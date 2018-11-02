using System;
using Modules.Abilityable.Ability;
using UnityEngine;

namespace Modules.Abilityable
{
    [Serializable]
    public class AbilityIcon : IAbilityIcon
    {
        [SerializeField] private Sprite _icon;

        public Sprite Icon
        {
            get { return _icon; }
        }
    }
}