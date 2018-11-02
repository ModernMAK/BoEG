using System.Collections.Generic;
using UnityEngine;

namespace Modules.Abilityable.Ability
{
    public abstract class Ability : ScriptableObject, IAbility,
        IAbilityData, IAbilityManacost, IAbilityCast, IAbilityCastRange, IAbilityIcon, IAbilityCooldown,
        IAbilityChannel, IAbilityLevel
    {
        public string Name;

//        public Sprite Icon;
//        public float Cooldown;

//        public int Level;
        public abstract void Terminate();


        protected GameObject Self { get; private set; }


        public virtual IAbility CreateInstance()
        {
            return Instantiate(this);
        }

        public virtual void Step(float deltaTick)
        {
        }

        public virtual void PreStep(float deltaStep)
        {
        }

        public virtual void PostStep(float deltaTick)
        {
        }

        public virtual void PhysicsStep(float deltaTick)
        {
        }

        public void Initialize(GameObject go)
        {
            Self = go;
            foreach (var module in Submodules)
                if (module != null)
                    module.Initialize(go, this);
//            if (ChannelData != null)
//                ChannelData.StopChannel(); //You can stop a channel, 
//            if (CooldownData != null)
//                CooldownData.ResetCooldown(); //but stopping a cooldown sounds like pausing; so we Reset it instead
            Initialize();
        }

        protected virtual void Initialize()
        {
        }

        protected virtual IEnumerable<AbilitySubmodule> Submodules
        {
            get
            {
                yield return ManacostData;
//                yield return IconData;
                yield return LevelData;
                yield return CastRangeData;
//                yield return ChannelData;
                yield return CooldownData;
            }
        }

        #region Manacost

        protected virtual AbilityManacost ManacostData
        {
            get { return null; }
        }

        public float ManaCost
        {
            get { return ManacostData != null ? ManacostData.ManaCost : 0f; }
        }

        public bool HasEnoughMana
        {
            get { return ManacostData == null || ManacostData.HasEnoughMana; }
        }

        #endregion

        #region Icon

        public Sprite Icon
        {
            get { return IconData.Icon; }
        }

        protected virtual AbilityIcon IconData
        {
            get { return null; }
        }

        #endregion

        #region Level

        protected virtual AbilityLevel LevelData
        {
            get { return null; }
        }

        public int Level
        {
            get { return LevelData != null ? LevelData.Level : 1; }
        }

        public int MaxLevel { 
            get { return LevelData != null ? LevelData.MaxLevel : 1; } }

        public void LevelUp()
        {
            if(LevelData != null)
                LevelData.LevelUp();
        }

        #endregion

        #region CastRange

        protected virtual AbilityCastRange CastRangeData
        {
            get { return null; }
        }

        public float CastRange
        {
            get { return CastRangeData != null ? CastRangeData.CastRange : 0f; }
        }

        public bool InCastRange(Vector3 point)
        {
            return CastRangeData == null || CastRangeData.InCastRange(point);
        }

        #endregion

        #region Cast

        public bool Preparing
        {
            get { return CastData != null && CastData.Preparing; }
        }

        public bool CanCast
        {
            get { return CastData != null && CastData.CanCast; }
        }

        public void Prepare()
        {
            if (CastData != null)
                CastData.Prepare();
        }

        public void CancelPrepare()
        {
            if (CastData != null)
                CastData.CancelPrepare();
        }

        public void Cast()
        {
            if (CastData != null)
                CastData.Cast();
        }

        public void Trigger()
        {
            if (CastData != null)
                CastData.Trigger();
        }

        public void GroundCast(Vector3 point)
        {
            if (CastData != null)
                CastData.GroundCast(point);
        }

        public void UnitCast(GameObject target)
        {
            if (CastData != null)
                CastData.UnitCast(target);
        }

        protected virtual AbilityCast CastData
        {
            get { return null; }
        }

        #endregion

        #region Channel

        public float MaxChannelDuration
        {
            get { return ChannelData != null ? ChannelData.MaxChannelDuration : 0f; }
        }

        public bool IsChanneling
        {
            get { return ChannelData != null && ChannelData.IsChanneling; }
        }

        public void StartChannel()
        {
            if (ChannelData != null)
                ChannelData.StartChannel();
        }

        public void StopChannel()
        {
            if (ChannelData != null)
                ChannelData.StopChannel();
        }

        public float ChannelProgress
        {
            get { return ChannelData != null ? ChannelData.ChannelProgress : 0f; }
        }

        public float ChannelProgressNormalized
        {
            get { return ChannelData != null ? ChannelData.ChannelProgressNormalized : 1f; }
        }

        protected virtual AbilityChannel ChannelData
        {
            get { return null; }
        }

        #endregion

        #region Cooldown

        protected virtual AbilityCooldown CooldownData
        {
            get { return null; }
        }

        public float Cooldown
        {
            get { return CooldownData != null ? CooldownData.Cooldown : 0f; }
        }

        public void StartCooldown()
        {
            if (CooldownData != null)
                CooldownData.StartCooldown();
        }

        public void ResetCooldown()
        {
            if (CooldownData != null)
                CooldownData.ResetCooldown();
        }

        public float CooldownRemaining
        {
            get { return CooldownData != null ? CooldownData.CooldownRemaining : 0f; }
        }

        public float CooldownRemainingNormalized
        {
            get { return CooldownData != null ? CooldownData.CooldownRemainingNormalized : 1f; }
        }

        public bool OffCooldown
        {
            get { return CooldownData == null || CooldownData.OffCooldown; }
        }

        #endregion

        public class AbilitySubmodule
        {
            protected GameObject Self { get; private set; }
            protected Ability Ability { get; private set; }

            public void Initialize(GameObject go, Ability ability)
            {
                Self = go;
                Ability = ability;
                Initialize();
            }

            protected virtual void Initialize()
            {
            }
        }
    }
}