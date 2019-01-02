using UnityEngine;

namespace Framework.Ability.Hero.WarpedMagi
{
    public class AbilityCooldown
    {
        public AbilityCooldown(float cooldown)
        {
            Cooldown = cooldown;
            CooldownLeft = 0f;
        }
        public float Cooldown { get; private set; }
        public float CooldownLeft { get; private set; }

        public void SetCooldown(float cooldown)
        {
            Cooldown = cooldown;
        }
        
        /// <summary>
        /// Value from 0 to 1; when Cooldown changes may linger on 1 while Cooldown 'catches up'
        /// </summary>
        public float CooldownNormal
        {
            get { return Mathf.Clamp01(CooldownLeft / Cooldown); }
        }

        public bool OnCooldown
        {
            get { return CooldownLeft > 0f; }
        }

        public void AdvanceTime(float deltaTime)
        {
            if(!OnCooldown)
                return;
            CooldownLeft -= deltaTime;
        }

        public void ResetCooldown()
        {
            CooldownLeft = 0f;
        }

        public void StartCooldown()
        {
            CooldownLeft = Cooldown;
        }
    }
}