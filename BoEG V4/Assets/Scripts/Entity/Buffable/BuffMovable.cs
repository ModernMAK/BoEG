using Entity.Buffable;

public class BuffMovable : Movable
{
    public override float MoveSpeed
    {
        get
        {
            var buffList = GetComponent<Buffable>().GetBuffs<IMovableBuff>();
            var buffData = buffList.SimplifyToBuff(buff => buff.MoveSpeedBuff);
            return buffData.ApplyToValue(base.MoveSpeed);
        }
    }

    public override float TurnSpeed
    {
        get
        {
            var buffList = GetComponent<Buffable>().GetBuffs<IMovableBuff>();
            var buffData = buffList.SimplifyToBuff(buff => buff.TurnSpeedBuff);
            return buffData.ApplyToValue(base.TurnSpeed);
        }
    }
}