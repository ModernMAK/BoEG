public interface IHealthableBuff : IBuff
{
    ValueBuffData HealthCapacityBuff { get; }
    ValueBuffData HealthBasicGenerationBuff { get; }
    ValueBuffData HealthCapacityGenerationBuff { get; }
    ValueBuffData HealthPoolGenerationBuff { get; }
    ValueBuffData HealthLostGenerationBuff { get; }
}


//public class Buff
//{
//}
//public class StatableBuff : Buff
//{
//    /// <summary>
//    /// Flat capacity Buff.
//    /// Stacks additively.
//    /// </summary>
//    protected float FlatStatCapBuff;
//    /// <summary>
//    /// Percentage Based Capacity Buff.
//    /// Stacks Multiplicitively.
//    /// </summary>
//    public float PercStatCapBuff;
//}
//public class RegenStatableBuff : StatableBuff
//{
//    /// <summary>
//    /// Flat Generation Buff.
//    /// Stacks additively.
//    /// </summary>
//    public float FlatStatGenBuff;
//    /// <summary>
//    /// Percentage Based Generation Buff.
//    /// Stacks multiplicitively.
//    /// </summary>
//    public float PercStatGenBuff;
//    /// <summary>
//    /// Capacity Based Percentage Generation Buff.
//    /// Stacks multiplicitively.
//    /// </summary>
//    public float CapStatGenBuff;
//    /// <summary>
//    /// Pool Based Percentage Generation Buff.
//    /// Stacks multiplicitively.
//    /// </summary>
//    public float PoolStatGenBuff;
//    /// <summary>
//    /// Stat Lost Based Percentage Generation Buff.
//    /// Stacks multiplicitively.
//    /// </summary>
//    public float LostStatGenBuff;
//}

//public class HealthableBuff : RegenStatableBuff
//{
//    public float FlatHealthCapBuff
//    {
//        get
//        {
//            return FlatStatGenBuff;
//        }
//        set
//        {
//            FlatStatGenBuff = value;
//        }
//    }
//}