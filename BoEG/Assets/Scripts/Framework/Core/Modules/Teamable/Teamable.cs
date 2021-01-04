using System;
using UnityEngine;

namespace MobaGame.Framework.Core.Modules
{
    [Serializable]
    public class Teamable : MonoBehaviour, ITeamable, IInitializable<TeamData>
    {
        [SerializeField] private TeamData _team;
        private event EventHandler<TeamData> _teamChanged;

        public void Initialize(TeamData module)
        {
            _team = module;
            OnTeamChanged(_team);
        }

        public TeamData Team => _team;

        public event EventHandler<TeamData> TeamChanged
        {
            add => _teamChanged += value;
            remove => _teamChanged -= value;
        }

        public void SetTeam(TeamData team)
        {
            _team = team;
            OnTeamChanged(team);
        }

        public bool SameTeam(ITeamable teamable)
        {
            return _team != null && _team == teamable.Team;
        }

        protected virtual void OnTeamChanged(TeamData e)
        {
            _teamChanged?.Invoke(this, e);
        }
    }
}