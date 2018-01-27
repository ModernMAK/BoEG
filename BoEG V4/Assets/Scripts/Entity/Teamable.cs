using UnityEngine;

public class Teamable : UnetBehaviour
{
    [SerializeField] private Team _team;

    public Team Team
    {
        get { return _team; }
        set { _team = value; }
    }

    public bool IsAlly(Teamable other)
    {
        return other.Team == Team && Team != Team.Hostile;
    }
}

public static class TeamableExt
{
    public static bool SafeIsAlly(this Teamable team, Teamable other, bool relation = false)
    {
        return other != null ? team.IsAlly(other) : relation;
    }
}

public enum Team
{
    //The Default team
    Hostile = 0,
    Red,
    Orange,
    Yellow,
    Green,
    Cyan,
    Blue,
    Purple,
    White
}

public enum Relation
{
    Ally,
    Enemy
}