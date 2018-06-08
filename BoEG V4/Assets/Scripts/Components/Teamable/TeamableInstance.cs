using Core;
using UnityEngine;

namespace Components.Teamable
{
    [System.Serializable]
    public class TeamableInstance : Module, ITeamableInstance
    {
        public TeamableInstance()
        {
            _team = null;
        }

        public TeamableInstance(TeamData team)
        {
            _team = team;
        }
        
        [SerializeField]
        private TeamData _team;
    
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