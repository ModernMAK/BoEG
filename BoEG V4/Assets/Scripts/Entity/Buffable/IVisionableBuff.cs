public interface IVisionableBuff : IBuff
{
    ValueBuffData NormVisionBuff { get; }
    ValueBuffData TrueVisionBuff { get; }
}