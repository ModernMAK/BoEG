namespace MobaGame.Framework.Core.Modules
{
    public interface IVisableData
    {
        bool HasSpotted { get; }
        bool HasInvisability { get; }
        bool HasHidden { get; }
    }
}