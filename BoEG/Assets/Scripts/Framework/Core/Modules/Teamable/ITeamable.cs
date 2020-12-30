using System;

namespace Modules.Teamable
{
    public interface ITeamable
    {
        TeamData Team { get; }
        event EventHandler<TeamData> TeamChanged;
        void SetTeam(TeamData team);
        bool SameTeam(ITeamable teamable);
    }
}