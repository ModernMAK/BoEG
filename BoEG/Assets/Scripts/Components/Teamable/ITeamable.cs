using System;

namespace Components.Teamable
{
    [Obsolete]
    public interface ITeamable
    {
        ITeamableInstance Teamable { get; }
    }
}