using UnityEngine;

namespace Entity.Buffable
{
    [RequireComponent(typeof(Buffable))]
    public class BuffHealthable : RegenStatable
    {
        public override float Capacity
        {
            get
            {
                var buffList = GetComponent<Buffable>().GetBuffs<IHealthableBuff>();
                var buffData = buffList.SimplifyToBuff(buff => buff.HealthCapacityBuff);
                return buffData.ApplyToValue(base.Capacity);
            }
        }

        public override float CapGen
        {
            get
            {
                var buffList = GetComponent<Buffable>().GetBuffs<IHealthableBuff>();
                var buffData = buffList.SimplifyToBuff(buff => buff.HealthCapacityGenerationBuff);
                return buffData.ApplyToValue(base.CapGen);
            }
        }

        public override float BasicGen
        {
            get
            {
                var buffList = GetComponent<Buffable>().GetBuffs<IHealthableBuff>();
                var buffData = buffList.SimplifyToBuff(buff => buff.HealthBasicGenerationBuff);
                return buffData.ApplyToValue(base.BasicGen);
            }
        }

        public override float LostGen
        {
            get
            {
                var buffList = GetComponent<Buffable>().GetBuffs<IHealthableBuff>();
                var buffData = buffList.SimplifyToBuff(buff => buff.HealthLostGenerationBuff);
                return buffData.ApplyToValue(base.LostGen);
            }
        }

        public override float PoolGen
        {
            get
            {
                var buffList = GetComponent<Buffable>().GetBuffs<IHealthableBuff>();
                var buffData = buffList.SimplifyToBuff(buff => buff.HealthPoolGenerationBuff);
                return buffData.ApplyToValue(base.PoolGen);
            }
        }
    }
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