namespace Modules.Teamable
{
    public interface ITeamable
    {
        TeamData Team { get; }
        void SetTeam(TeamData team);
        bool SameTeam(ITeamable teamable);
    }
}