namespace MobaGame.Framework.Core.Modules
{
    public interface IArmor
    {
        float Block { get; }
        float Resistance { get; }
        bool Immunity { get; }
    }
}