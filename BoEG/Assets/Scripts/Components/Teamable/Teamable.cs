using UnityEngine;

namespace Components.Teamable
{
    [System.Serializable]
    public class Teamable : Module, ITeamable
    {
        public Teamable()
        {
            _team = null;
        }

        public Teamable(TeamData team)
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