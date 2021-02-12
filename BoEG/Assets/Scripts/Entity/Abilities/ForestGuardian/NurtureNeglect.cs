using MobaGame.Framework.Core;
using MobaGame.Framework.Core.Modules;
using MobaGame.Framework.Core.Modules.Ability;
using MobaGame.Framework.Core.Trigger;
using MobaGame.Framework.Types;
using System;
using System.Collections.Generic;
using MobaGame.Framework.Core.Modules.Ability.Helpers;
using UnityEngine;

namespace MobaGame.Entity.Abilities.ForestGuardian
{
    [CreateAssetMenu(menuName = "Ability/ForestGuardian/Nurture-Neglect")]
    public class NurtureNeglect : AbilityObject
    {
#pragma warning disable 0649
        private BlackRoseCurse _blackRose;
        [SerializeField]
        private Sprite _rrIcon;
        [SerializeField]
        private Sprite _brIcon;
        
        
        [Header("Nurture Aura")] [SerializeField]
        private float _nurtureRange;

        [SerializeField] private float _nurtureHealingAmplification;

        [Header("Neglect Aura")] [SerializeField]
        private float _neglectRange;

        [SerializeField] private float _neglectHealingNegation;


        private List<IHealable> _nurtureTargets;
        private List<IHealable> _neglectTargets;
        private TriggerHelper<SphereCollider> _nurtureAura;
        private TriggerHelper<SphereCollider> _neglectAura;
#pragma warning restore 0649


        public SimpleAbilityView View { get; set; }

        public AbilityPredicateBuilder CheckBuilder { get; set; }
        private TeamableChecker _rrTeamChecker;
        private TeamableChecker _brTeamChecker;
        
        public override void Initialize(Actor data)
        {
            base.Initialize(data);
            Modules.Abilitiable.TryGetAbility(out _blackRose);

            _nurtureAura = TriggerUtility.GetOrCreateTrigger<SphereCollider>(data, "Nurture Aura");
            _nurtureAura.Collider.radius = _nurtureRange;
            _nurtureAura.Trigger.Enter += Nurture_Enter;
            _nurtureAura.Trigger.Exit += Nurture_Exit;

            _neglectAura = TriggerUtility.GetOrCreateTrigger<SphereCollider>(data, "Neglect Aura");
            _neglectAura.Collider.radius = _neglectRange;
            _neglectAura.Trigger.Enter += Neglect_Enter;
            _neglectAura.Trigger.Exit += Neglect_Exit;

            _neglectAura.Collider.enabled = false;
            _nurtureAura.Collider.enabled = true;

            _nurtureTargets = new List<IHealable>();
            _neglectTargets = new List<IHealable>();
            
            _rrTeamChecker = TeamableChecker.AllyOnly(Modules.Teamable);
            _brTeamChecker = TeamableChecker.NonAllyOnly(Modules.Teamable);
            CheckBuilder = new AbilityPredicateBuilder(data)
            {
                AllowSelf = false
            };
            View = new SimpleAbilityView()
            {
                Icon = _rrIcon,
                Cooldown = CheckBuilder.Cooldown,
                StatCost = CheckBuilder.MagicCost,
            };
            CheckBuilder.RebuildChecks();
        }

        public override void Setup()
        {
            _blackRose.Toggled += BlackRose_Toggled;
        }

        public override IAbilityView GetAbilityView() => View;

        private void BlackRose_Toggled(object sender, ChangedEventArgs<bool> e)
        {
            ClearNeglectTargets();
            ClearNurtureTargets();
            var isBlackRose = e.After;
            View.Icon = isBlackRose ? _brIcon : _rrIcon;
            _neglectAura.Collider.enabled = isBlackRose;
            _nurtureAura.Collider.enabled = !isBlackRose;
            CheckBuilder.AllowSelf = !isBlackRose;
            CheckBuilder.RebuildChecks();
                
            if (isBlackRose)
                RecalculateNeglectTargets();
            else
                RecalculateNurtureTargets();
        }

        private void Neglect_Exit(object sender, TriggerEventArgs e)
        {
            var col = e.Collider;
            if (!AbilityHelper.TryGetActor(col, out var actor))
                return;
            if (!actor.TryGetModule<IHealable>(out var healable))
                return;
            healable.HealingModifiers -= NeglectModifier;
            _neglectTargets.Remove(healable);
        }

        private void Neglect_Enter(object sender, TriggerEventArgs e)
        {
            var col = e.Collider;
            if (!AbilityHelper.TryGetActor(col, out var actor))
                return;
            if (!actor.TryGetModule<IHealable>(out var healable))
                return;
            if(!CheckBuilder.AllowTarget(actor))
                return;
            healable.HealingModifiers += NeglectModifier;
            _neglectTargets.Add(healable);
        }

        private void ClearNeglectTargets()
        {
            foreach (var healable in _neglectTargets)
                healable.HealingModifiers -= NeglectModifier;
            _neglectTargets.Clear();
        }

        private void RecalculateNeglectTargets()
        {
            var colliders = _neglectAura.Collider.OverlapSphere();
            foreach (var col in colliders)
            {
                if (!AbilityHelper.TryGetActor(col, out var actor))
                    return;
                if (!actor.TryGetModule<IHealable>(out var healable))
                    return;
                if(!CheckBuilder.AllowTarget(actor))
                    return;
                healable.HealingModifiers += NeglectModifier;
                _neglectTargets.Add(healable);
            }
        }

        private void Nurture_Exit(object sender, TriggerEventArgs e)
        {
            var col = e.Collider;
            if (!AbilityHelper.TryGetActor(col, out var actor))
                return;
            if (!actor.TryGetModule<IHealable>(out var healable))
                return;
            healable.HealingModifiers -= NurtrueModifier;
            _nurtureTargets.Remove(healable);
        }

        private void Nurture_Enter(object sender, TriggerEventArgs e)
        {
            var col = e.Collider;
            if (!AbilityHelper.TryGetActor(col, out var actor))
                return;
            if (!actor.TryGetModule<IHealable>(out var healable))
                return;
            if(!CheckBuilder.AllowTarget(actor))
                return;
            healable.HealingModifiers += NurtrueModifier;
            _nurtureTargets.Add(healable);
        }

        private void NurtrueModifier(object sender, ChangableEventArgs<SourcedHeal> e)
        {
            var healValue = e.After.Value;
            var bonusHeal = healValue * _nurtureHealingAmplification;
            //Do not let heal become negative
            healValue = Mathf.Min(healValue + bonusHeal, 0);
            e.After = e.After.SetHeal(healValue);
        }

        private void NeglectModifier(object sender, ChangableEventArgs<SourcedHeal> e)
        {
            var healValue = e.After.Value;
            var negatedHeal = healValue * _neglectHealingNegation;
            //Do not let heal become negative, and make sure its not bigger then the original value.
            healValue = Mathf.Clamp(healValue - negatedHeal, 0, healValue);
            e.After = e.After.SetHeal(healValue);
        }

        private void ClearNurtureTargets()
        {
            foreach (var healable in _nurtureTargets)
                healable.HealingModifiers -= NurtrueModifier;
            _nurtureTargets.Clear();
        }

        private void RecalculateNurtureTargets()
        {
            var colliders = _nurtureAura.Collider.OverlapSphere();
            foreach (var col in colliders)
            {
                if (!AbilityHelper.TryGetActor(col, out var actor))
                    return;
                if (!actor.TryGetModule<IHealable>(out var healable))
                    return;
                if(!CheckBuilder.AllowTarget(actor))
                    return;
                healable.HealingModifiers += NurtrueModifier;
                _nurtureTargets.Add(healable);
            }
        }
    }
}