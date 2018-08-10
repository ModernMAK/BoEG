namespace Modules.Armorable
{
    public interface IArmorableData
    {
        ArmorData Physical { get; }
        ArmorData Magical { get; }
    }
}