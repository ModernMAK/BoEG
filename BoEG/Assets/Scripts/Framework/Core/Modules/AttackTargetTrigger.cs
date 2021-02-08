using System;
using System.Collections.Generic;
using Framework.Core;
using MobaGame.Framework.Core.Modules.Ability;
using MobaGame.Framework.Core.Trigger;
using MobaGame.Framework.Types;
using UnityEngine;

namespace MobaGame.Framework.Core.Modules
{
    //Named becuse it uses the 'Targetable.AllowAttackTargets' this also works for Aggroable 
    public class AttackTargetTrigger<T> : IDisposable where T : Collider
    {
        public Actor Actor { get; }
        public ITeamable Teamable { get; }
        public TriggerHelper<T> Trigger { get; }

        private readonly List<Actor> _targets;
        public IReadOnlyList<Actor> Targets => _targets;

        public AttackTargetTrigger(Actor actor, TriggerHelper<T> helper, ITeamable teamable = default)
        {
            _targets = new List<Actor>();
            Trigger = helper;
            Actor = actor;
            Teamable = teamable;
            if (Teamable != null)
                Teamable.TeamChanged += OnMyTeamChanged;
            Trigger.Trigger.Enter += TriggerOnEnter;
            Trigger.Trigger.Exit += TriggerOnExit;
        }


        private void OnMyTeamChanged(object sender, ChangedEventArgs<TeamData> e)
        {
            InstantlyRebuildTargets();
        }

        private void OnTargetTeamChanged(object sender, ChangedEventArgs<TeamData> e)
        {
            InstantlyRebuildTargets();
        }

        private void InstantlyRebuildTargets()
        {
            while (_targets.Count > 0)
            {
                InternalRemoveActor(_targets[0]);
            }

            var colliders = Trigger.Trigger.OverlapCollider((int) LayerMaskHelper.Entity);
            foreach (var col in colliders)
            {
                if (AbilityHelper.TryGetActor(col, out var actor))
                    InternalAddActor(actor);
            }
        }


        void InternalAddActor(Actor actor)
        {
            if (actor == Actor)
                return;

            if (_targets.Contains(actor))
                return;

            if (!actor.TryGetModule<IDamageable>(out _))
                return;

            if (actor.TryGetModule<IKillable>(out var killable))
                killable.Died += TargetDied;

            if (actor.TryGetModule<ITeamable>(out var teamable))
                teamable.TeamChanged += OnTargetTeamChanged;

            if (actor.TryGetModule<ITargetable>(out var targetable))
                targetable.AttackTargetingChanged += OnTargetAttackTargetingChanged;

            //Because of event registration, we hold off on actually uisng the vriable until now
            if (targetable?.AllowAttackTargets ?? false)
                return;

            if (Teamable?.SameTeam(teamable) ?? false)
                return;

            _targets.Add(actor);
        }

        private void OnTargetAttackTargetingChanged(object sender, EventArgs e)
        {
            InstantlyRebuildTargets();
        }

        void InternalRemoveActor(Actor actor)
        {
            if (actor.TryGetModule<IKillable>(out var killable))
                killable.Died -= TargetDied;

            if (actor.TryGetModule<ITeamable>(out var teamable))
                teamable.TeamChanged -= OnTargetTeamChanged;

            if (actor.TryGetModule<ITargetable>(out var targetable))
                targetable.AttackTargetingChanged -= OnTargetAttackTargetingChanged;

            _targets.Remove(actor);
        }


        private void TriggerOnExit(object sender, TriggerEventArgs e)
        {
            var go = e.Collider.gameObject;


            if (!go.TryGetComponent<Actor>(out var actor))
                return;

            InternalRemoveActor(actor);
        }

        private void TriggerOnEnter(object sender, TriggerEventArgs e)
        {
            var go = e.Collider.gameObject;
            if (!go.TryGetComponent<Actor>(out var actor))
                return;

            InternalAddActor(actor);
        }


        private void TargetDied(object sender, DeathEventArgs e)
        {
            InternalRemoveActor(e.Self);
        }

        public void Dispose()
        {
            if (Teamable != null)
                Teamable.TeamChanged -= OnMyTeamChanged;
            Trigger.Trigger.Enter -= TriggerOnEnter;
            Trigger.Trigger.Exit -= TriggerOnExit;
        }
    }
}