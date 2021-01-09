using System;

namespace MobaGame.Framework.Core.Modules
{
    public interface ITeamable
    {
        TeamData Team { get; }

        event EventHandler<ChangedEventArgs<TeamData>> TeamChanged; 
        void SetTeam(TeamData team);
        bool SameTeam(ITeamable teamable);
    }
}