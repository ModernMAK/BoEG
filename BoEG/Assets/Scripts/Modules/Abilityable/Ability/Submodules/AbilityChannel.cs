using System;
using UnityEngine;

namespace Modules.Abilityable.Ability
{
    [Serializable]
    public class AbilityChannel : IAbilityChannel
    {
        private float _myChannelStart;

        public virtual float MaxChannelDuration
        {
            get { return 0; }
        }

        public bool IsChanneling { get; private set; }

        public void StartChannel()
        {
            _myChannelStart = Time.time;
            IsChanneling = true;
        }

        public void StopChannel()
        {
            _myChannelStart = 0f;
            IsChanneling = false;
        }

        public float ChannelProgress
        {
            get { return IsChanneling ? 0f : Mathf.Min((Time.time - _myChannelStart), MaxChannelDuration); }
        }

        //0 -> the channel began (Or is not being channeled)
        //1 -> the channel completed
        public float ChannelProgressNormalized
        {
            get
            {
                var mcd = MaxChannelDuration;
                return mcd > 0f ? (ChannelProgress / mcd) : 1f;
            }
        }
    }
}