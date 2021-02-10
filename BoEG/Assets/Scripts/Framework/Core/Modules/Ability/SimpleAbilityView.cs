using System;
using MobaGame.Entity.Abilities;
using MobaGame.Framework.Types;
using UnityEngine;

namespace MobaGame.Framework.Core.Modules.Ability
{
    public class SimpleAbilityView : IAbilityView
    {

        private Sprite _icon;
        public Sprite Icon
        {
            get => _icon;
            set
            {
                _icon = value;
                OnChanged();
            } 
        }


        private CooldownAbilityView _cooldown;
        public CooldownAbilityView Cooldown
        {
            get => _cooldown;
            set
            {
                if (_cooldown != null)
                    _cooldown.Changed -= OnInternalChanged;
                _cooldown = value;
                if (_cooldown != null)
                    _cooldown.Changed += OnInternalChanged;
            }
        }

        private void OnInternalChanged(object sender, EventArgs e) => OnChanged();

        ICooldownAbilityView IAbilityView.Cooldown => Cooldown;

        private StatCostAbilityView _statCost;
        public StatCostAbilityView StatCost { 
            get => _statCost;
            set
            {
                if (_statCost != null)
                    _statCost.Changed -= OnInternalChanged;
                _statCost = value;
                if (_statCost != null)
                    _statCost.Changed += OnInternalChanged;
            } 
        }
        IStatCostAbilityView IAbilityView.StatCost => StatCost;


        private ToggleableAbilityView _toggleable;
        public ToggleableAbilityView Toggleable { 
            get => _toggleable;
            set
            {
                if (_toggleable != null)
                    _toggleable.Changed -= OnInternalChanged;
                _toggleable = value;
                if (_toggleable != null)
                    _toggleable.Changed += OnInternalChanged;
            }
            
        }
        IToggleableAbilityView IAbilityView.Toggleable => Toggleable;

        public event EventHandler Changed;
        public void OnChanged() => Changed?.Invoke(this,EventArgs.Empty);
    }

    public class CooldownAbilityView : ICooldownAbilityView
    {
        public CooldownAbilityView(DurationTimer timer)
        {
            Timer = timer;
        }

        private DurationTimer _timer;
        public DurationTimer Timer
        {
            get => _timer;
            set
            {
                if (_timer != null)
                    _timer.Changed -= InteralOnChanged;
                _timer = value;
                if (_timer != null)
                    _timer.Changed += InteralOnChanged;
                OnChanged();
            } }

        private void InteralOnChanged(object sender, EventArgs e) => OnChanged();

        public void StartCooldown() => Timer.Reset();

        public void AdvanceTime(float deltaTime)
        {
            Timer.AdvanceTimeIfNotDone(deltaTime);
        }

        public float Cooldown => Timer.Duration;

        public float CooldownRemaining => Timer.RemainingTime;

        public float CooldownNormal => Timer.ElapsedTime / Timer.Duration;

        public bool OnCooldown => CooldownRemaining > 0f;
        public event EventHandler Changed;
        private void OnChanged() => Changed?.Invoke(this,EventArgs.Empty);
    }
}