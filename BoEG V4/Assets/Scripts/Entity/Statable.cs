using System;
using UnityEngine;
using UnityEngine.Networking;

public class Statable : UnetBehaviour
{
    [SerializeField] private float _valueCapacity = 1f;

    [SerializeField] private float _valueNormalized = 1f;

    /// <summary>
    ///     The current value, between 0 and Capacity.
    /// </summary>
    public float Value
    {
        get { return ValueNormalized * Capacity; }
        protected set { ValueNormalized = value / Capacity; }
    }


    /// <summary>
    ///     The Value normalized between 0 and Capacity.
    ///     Expected to be [0,1].
    /// </summary>
    public float ValueNormalized
    {
        get { return _valueNormalized; }
        protected set
        {
            if (value != _valueNormalized)
                SetDirtyBit(1);
            _valueNormalized = value;
        }
    }

    /// <summary>
    ///     The Maximum Capacity.
    /// </summary>
    public float BaseCapacity
    {
        get { return _valueCapacity; }
        protected set
        {
            if (value <= 0f)
                throw new ArgumentException("Cannot be less than or equal to 0!", "value");
            if (value != _valueCapacity)
                SetDirtyBit(2);
            _valueCapacity = value;
        }
    }

    public virtual float Capacity
    {
        get { return BaseCapacity; }
    }

    protected override void ModuleSerialize(ModuleWriter writer)
    {
        base.ModuleSerialize(writer);
        writer.WriteIf(ValueNormalized, 1);
        writer.WriteIf(BaseCapacity, 2);
    }

    protected override void ModuleDeserialize(NetworkReader reader, uint mask, bool readMask = false)
    {
        if (readMask)
            mask = reader.ReadUInt32();

        if (mask.GetBit(1))
            ValueNormalized = reader.ReadSingle();

        if (mask.GetBit(2))
            BaseCapacity = reader.ReadSingle();

        base.ModuleDeserialize(reader, mask);
    }


    //Clamps from 0 to Capacity
    protected float ClampToCapacity(float value)
    {
        return Mathf.Clamp(value, 0f, Capacity);
    }
    //{
    //    bool wrote = initialState;
    //    if (initialState)
    //    {
    //        writer.Write(_ValueNormalized);
    //        writer.Write(_ValueCapacity);
    //    }
    //    else
    //    {


    //public override bool OnSerialize(NetworkWriter writer, bool initialState)

    //        writer.Write(syncVarDirtyBits);
    //        if (BitOps.GetBit(syncVarDirtyBits,0))
    //            writer.Write(_ValueNormalized);
    //        if (BitOps.GetBit(syncVarDirtyBits, 1))
    //            writer.Write(_ValueCapacity);

    //    }
    //    if (wrote)
    //        ClearAllDirtyBits();
    //    return wrote;
    //}
    //public override void OnDeserialize(NetworkReader reader, bool initialState)
    //{
    //    if (initialState)
    //    {
    //        _ValueNormalized = reader.ReadSingle();
    //        _ValueCapacity = reader.ReadSingle();
    //    }
    //    else
    //    {
    //        var mask = reader.ReadUInt32();
    //        if (BitOps.GetBit(mask, 0))
    //            _ValueNormalized = reader.ReadSingle();
    //        if (BitOps.GetBit(mask, 1))
    //            _ValueCapacity = reader.ReadSingle();
    //    }
    //}
}


//public class Buff : ModuleNetworkBehaviour
//{

//}

//public class StatableBuff : Buff
//{
//    [SerializeField]
//    private float _CapacityFlatBuff;
//    [SerializeField]
//    private float _CapacityPercBuff;

//    public float CapacityFlatBuff
//    {
//        get
//        {
//            return _CapacityFlatBuff;
//        }

//        set
//        {
//            if (_CapacityFlatBuff != value)
//                SetDirtyBit(1);
//            _CapacityFlatBuff = value;
//        }
//    }

//    public float CapacityPercBuff
//    {
//        get
//        {
//            return _CapacityPercBuff;
//        }

//        set
//        {
//            if (_CapacityPercBuff != value)
//                SetDirtyBit(2);
//            _CapacityPercBuff = value;
//        }
//    }
//}