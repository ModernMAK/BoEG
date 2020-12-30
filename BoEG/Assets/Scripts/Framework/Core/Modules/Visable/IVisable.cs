namespace Old.Modules.Visable
{
    public interface IVisable
    {
        bool IsInvisible { get; }
        bool IsHidden { get; }
        bool IsSpotted { get; }
    }
}