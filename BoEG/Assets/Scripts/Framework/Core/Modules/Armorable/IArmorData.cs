namespace Framework.Core.Modules
{
    public interface IArmorData
    {
        float Block { get; }
        float Resist { get; }
        bool Immune { get; }
    }
}