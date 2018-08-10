using System;
using UnityEngine;

namespace Modules.Teamable
{
    [Serializable]
    public class Teamable : Module, ITeamable
    {
        public Teamable(GameObject self, TeamData team = null) : base(self)
        {
            _team = team;
        }

        [SerializeField] private TeamData _team;

        public TeamData Team
        {
            get { return _team; }
        }

        public void SetTeam(TeamData team)
        {
            _team = team;
        }
    }
}