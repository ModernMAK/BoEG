using Core;

namespace Components.Teamable
{
    public interface ITeamableInstance
    {
        TeamData Team { get; }
        void SetTeam(TeamData team);
    }
}