namespace MobaGame.Framework.Core.Modules
{
    public interface IArmorableData
    {
        ArmorData Physical { get; }
        ArmorData Magical { get; }
    }
}