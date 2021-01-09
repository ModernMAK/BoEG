using System;
using System.Collections.Generic;
using MobaGame.Framework.Core.Modules.Ability;
using MobaGame.Framework.Core.Trigger;
using MobaGame.Framework.Types;
using UnityEngine;

namespace MobaGame.Framework.Core.Modules
{
    public class TargetTrigger<T> : IDisposable where T : Collider
    {
        public Actor Actor { get; }
        public ITeamable Teamable { get; }
        public TriggerHelper<T> Trigger { get; }

        private List<Actor> _targets;
        public IReadOnlyList<Actor> Targets => _targets;

        public TargetTrigger(Actor actor, TriggerHelper<T> helper, ITeamable teamable = default)
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


        private void OnMyTeamChanged(object sender,  ChangedEventArgs<TeamData> e)
        {
            InstantlyRebuildTargets();
        }

        private void OnTargetTeamChanged(object sender,  ChangedEventArgs<TeamData> e)
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

            if (!actor.TryGetComponent<IDamageTarget>(out _))
                return;

            if (actor.TryGetComponent<IHealthable>(out var healthable))
                healthable.Died += TargetDied;

            if (actor.TryGetComponent<ITeamable>(out var teamable))
                teamable.TeamChanged += OnTargetTeamChanged;

            if (Teamable?.SameTeam(teamable) ?? false)
                return;

            _targets.Add(actor);
        }

        void InternalRemoveActor(Actor actor)
        {
            if (actor.TryGetComponent<IHealthable>(out var healthble))
                healthble.Died -= TargetDied;

            if (actor.TryGetComponent<ITeamable>(out var teamable))
                teamable.TeamChanged -= OnTargetTeamChanged;

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