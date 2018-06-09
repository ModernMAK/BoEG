namespace Components.Teamable
{
    public interface ITeamable
    {
        TeamData Team { get; }
        void SetTeam(TeamData team);
    }
}