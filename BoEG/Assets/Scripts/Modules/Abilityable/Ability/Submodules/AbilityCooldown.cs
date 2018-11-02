using System;
using UnityEngine;

namespace Modules.Abilityable.Ability
{
    [Serializable]
    public abstract class AbilityCooldown : Ability.AbilitySubmodule, IAbilityCooldown
    {
        private float _myCooldownStart;

        public abstract float Cooldown { get; }

        public void StartCooldown()
        {
            _myCooldownStart = Time.time;
        }

        public void ResetCooldown()
        {
            _myCooldownStart = 0f;
        }

        public float CooldownRemaining
        {
            get { return Mathf.Max(Cooldown - (Time.time - _myCooldownStart), 0f); }
        }

        //1 -> the full cooldown remains
        //0 -> the cooldown was completed
        public float CooldownRemainingNormalized
        {
            get
            {
                var cd = Cooldown;
                return cd > 0f ? (CooldownRemaining / cd) : 0f;
            }
        }

        public bool OffCooldown
        {
            get { return Cooldown <= 0f || CooldownRemaining <= 0f; }
        }
        [Serializable]
        public class Flat : AbilityCooldown
        {
            [SerializeField] private float _cooldown;

            public override float Cooldown
            {
                get { return _cooldown; }
            }
        }
        [Serializable]
        public class Scalar : AbilityCooldown
        {
            [SerializeField] private float[] _cooldown;

            public override float Cooldown
            {
                get { return Ability.GetLeveledData(_cooldown); }
            }
        }
    }


}