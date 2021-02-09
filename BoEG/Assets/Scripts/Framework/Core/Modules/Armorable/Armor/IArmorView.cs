namespace MobaGame.Framework.Core.Modules
{
    public interface IArmorView : IView
    {
        float Block { get; }
        float Resistance { get; }
        bool Immunity { get; }

    }
}