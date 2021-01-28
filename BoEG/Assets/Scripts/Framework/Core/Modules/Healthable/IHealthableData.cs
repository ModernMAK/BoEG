namespace MobaGame.Framework.Core.Modules
{
    public interface IHealthableData
    {
        float Generation { get; }
        float Capacity { get; }
        float GenerationGain { get; }
        float CapacityGain { get; }
    }
}