namespace MobaGame.Framework.Core.Modules
{
    public interface IVisable
    {
        bool IsInvisible { get; }
        bool IsHidden { get; }
        bool IsSpotted { get; }
    }
}