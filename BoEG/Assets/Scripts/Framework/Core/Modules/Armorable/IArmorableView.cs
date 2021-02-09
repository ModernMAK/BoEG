namespace MobaGame.Framework.Core.Modules
{
    public interface IArmorableView : IView
    {
        IArmorView Physical { get; }
        IArmorView Magical { get; }
    }
}