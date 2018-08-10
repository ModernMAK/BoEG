namespace Old.Entity.Modules.Levelable
{
    public interface ILevelableData
    {
        int InitialLevel { get; }
        int MaxLevel { get; }
        int[] ExperienceCurve { get; }
    }
}