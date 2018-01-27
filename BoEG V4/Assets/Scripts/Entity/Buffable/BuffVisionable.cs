using Entity;
using Entity.Buffable;

public class BuffVisionable : Visionable
{
    public override float NormVisRange
    {
        get
        {
            var buffList = GetComponent<Buffable>().GetBuffs<IVisionableBuff>();
            var buffData = buffList.SimplifyToBuff(buff => buff.NormVisionBuff);
            return buffData.ApplyToValue(base.NormVisRange);
        }
    }

    public override float TrueVisRange
    {
        get
        {
            var buffList = GetComponent<Buffable>().GetBuffs<IVisionableBuff>();
            var buffData = buffList.SimplifyToBuff(buff => buff.TrueVisionBuff);
            return buffData.ApplyToValue(base.TrueVisRange);
        }
    }
}