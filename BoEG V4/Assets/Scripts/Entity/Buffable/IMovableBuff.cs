public interface IMovableBuff : IBuff
{
    ValueBuffData MoveSpeedBuff { get; }
    ValueBuffData TurnSpeedBuff { get; }
}