using UnityEngine;
using UnityEngine.Networking;

public class RegenStatable : Statable
{
    [SerializeField] private float _BaseFlatGen, _BasePoolGen, _BaseCapGen, _BaseLostGen;

    public float TotalGenerated
    {
        get
        {
            return BasicGen +
                   PoolGen * Value +
                   LostGen * (1f - ValueNormalized) * Capacity +
                   CapGen * Capacity;
        }
    }

    public float BaseFlatGen
    {
        get { return _BaseFlatGen; }
        set
        {
            if (_BaseFlatGen != value)
                SetDirtyBit(3);
            _BaseFlatGen = value;
        }
    }

    public virtual float BasicGen
    {
        get { return BaseFlatGen; }
    }

    public float BasePoolGen
    {
        get { return _BasePoolGen; }
        set
        {
            if (_BaseFlatGen != value)
                SetDirtyBit(4);
            _BasePoolGen = value;
        }
    }

    public virtual float PoolGen
    {
        get { return BasePoolGen; }
    }

    public float BaseCapGen
    {
        get { return _BaseCapGen; }
        set
        {
            if (_BaseFlatGen != value)
                SetDirtyBit(5);
            _BaseCapGen = value;
        }
    }

    public virtual float CapGen
    {
        get { return BaseCapGen; }
    }

    public float BaseLostGen
    {
        get { return _BaseLostGen; }
        set
        {
            if (_BaseFlatGen != value)
                SetDirtyBit(6);
            _BaseLostGen = value;
        }
    }

    public virtual float LostGen
    {
        get { return BaseLostGen; }
    }

    protected override void ModuleSerialize(ModuleWriter writer)
    {
        base.ModuleSerialize(writer);
        writer.WriteIf(BaseCapGen, 3);
        writer.WriteIf(BaseFlatGen, 4);
        writer.WriteIf(BaseLostGen, 5);
        writer.WriteIf(BasePoolGen, 6);
    }

    protected override void ModuleDeserialize(NetworkReader reader, uint mask, bool readMask = false)
    {
        if (readMask)
            mask = reader.ReadUInt32();

        if (mask.GetBit(3))
            BaseCapGen = reader.ReadSingle();

        if (mask.GetBit(4))
            BaseCapGen = reader.ReadSingle();

        if (mask.GetBit(5))
            BaseCapGen = reader.ReadSingle();

        if (mask.GetBit(6))
            BaseCapGen = reader.ReadSingle();

        base.ModuleDeserialize(reader, mask);
    }

    public void Generate(float gen, bool ignoreTime = false)
    {
        Value = ClampToCapacity(Value + gen * (ignoreTime ? 1f : Time.deltaTime));
    }

    protected override void Update()
    {
        if (isServer)
            Generate(TotalGenerated);
    }
}