﻿using System;
using UnityEngine;

namespace MobaGame.Framework.Core.Modules
{
    public class Teamable : ActorModule, ITeamable, IInitializable<TeamData>
    {
        public Teamable(Actor actor, TeamData team = default) : base(actor)
        {
            Initialize(team);
        }

        [SerializeField] private TeamData _team;
        private event EventHandler<ChangedEventArgs<TeamData>> _teamChanged;

        public void Initialize(TeamData data)
        {
            Team = data;
        }

        public TeamData Team
        {
            get => _team;
            set
            {
                var team = _team;
                var changed = (value != team);
                _team = value;
                if(changed)
                    OnTeamChanged(new ChangedEventArgs<TeamData>(team,value));

            }
        }

        public event EventHandler<ChangedEventArgs<TeamData>> TeamChanged
        {
            add => _teamChanged += value;
            remove => _teamChanged -= value;
        }

        public void SetTeam(TeamData team)
        {
            Team = team;
        }

        public bool SameTeam(ITeamable teamable)
        {
            return _team != null && _team == teamable.Team;
        }

        protected virtual void OnTeamChanged(ChangedEventArgs<TeamData> e)
        {
            _teamChanged?.Invoke(this, e);
        }

		public TeamRelation GetRelation(ITeamable other)
		{
            if (this == other)
                return TeamRelation.Ally;
            if (_team == null || other.Team == null)
                return TeamRelation.Neutral;
			return _team != other.Team ? TeamRelation.Enemy : TeamRelation.Ally;
		}
	}
}