using UnityEngine.Networking;

public class UnetBehaviour : NetworkBehaviour
{
    protected virtual void Awake()
    {
    }

    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {
    }

    public override bool OnSerialize(NetworkWriter writer, bool initialState)
    {
        var mask = syncVarDirtyBits;
        if (initialState)
            mask |= ~0u;
        var mWriter = new ModuleWriter(writer, mask);
        ModuleSerialize(mWriter);
        return mWriter.Wrote;
    }

    public override void OnDeserialize(NetworkReader reader, bool initialState)
    {
        var mask = syncVarDirtyBits;
        //if (initialState)
        //    mask |= ~0u;

        //Deserialize doesnt care about initialstate

        ModuleDeserialize(reader, mask, true);
    }

    protected virtual void ModuleSerialize(ModuleWriter writer)
    {
    }

    protected virtual void ModuleDeserialize(NetworkReader reader, uint mask, bool readMask = false)
    {
        //Do nothing
    }

    public class ModuleWriter
    {
        public ModuleWriter(NetworkWriter writer, uint mask)
        {
            Writer = writer;
            Mask = mask;
            Writer.Write(Mask);
            Wrote = false; //Mask alone does not count
        }

        public uint Mask { get; private set; }
        public NetworkWriter Writer { get; private set; }
        public bool Wrote { get; private set; }

        public void WriteIf(bool value, int bit)
        {
            if (!Mask.GetBit(bit))
                return;
            Writer.Write(value);
            Wrote = true;
        }

        public void WriteIf(uint value, int bit)
        {
            if (!Mask.GetBit(bit))
                return;
            Writer.Write(value);
            Wrote = true;
        }

        public void WriteIf(byte value, int bit)
        {
            if (!Mask.GetBit(bit))
                return;
            Writer.Write(value);
            Wrote = true;
        }

        public void WriteIf(int value, int bit)
        {
            if (!Mask.GetBit(bit))
                return;
            Writer.Write(value);
            Wrote = true;
        }

        public void WriteIf(float value, int bit)
        {
            if (!Mask.GetBit(bit))
                return;
            Writer.Write(value);
            Wrote = true;
        }

        //TODO Impliment More/Rest
    }
}


////32 Fields
//public struct DirtyLongMask
//{
//    public long Mask { get; set; }
//    public bool GetMask(int i)
//    {
//        return (Mask & (1L << i)) == Mask;
//    }
//    public void SetMask(int i, bool dirty = true)
//    {
//        if (dirty)
//            Mask |= (1L << i);
//        else
//            Mask &= ~(1L << i);
//    }
//    public void ClearMask()
//    {
//        Mask = 0;
//    }
//    public static implicit operator long(DirtyLongMask m)
//    {
//        return m.Mask;
//    }
//    public static explicit operator DirtyLongMask(long m)
//    {
//        return new DirtyLongMask() { Mask = m };
//    }
//}
////16 Fields
//public struct DirtyIntMask
//{
//    public int Mask { get; set; }
//    public bool GetMask(int i)
//    {
//        return (Mask & (1 << i)) == Mask;
//    }
//    public void SetMask(int i, bool dirty = true)
//    {
//        if (dirty)
//            Mask |= (1 << i);
//        else
//            Mask &= ~(1 << i);
//    }
//    public void ClearMask()
//    {
//        Mask = 0;
//    }
//    public static implicit operator int(DirtyIntMask m)
//    {
//        return m.Mask;
//    }
//    public static explicit operator DirtyIntMask(int m)
//    {
//        return new DirtyIntMask() { Mask = m };
//    }
//}
////8 Fields
//public struct DirtyByteMask
//{
//    public byte Mask { get; set; }
//    public bool GetMask(int i)
//    {
//        return (Mask & (1 << i)) == Mask;
//    }
//    public void SetMask(int i, bool dirty = true)
//    {
//        if (dirty)
//            Mask |= (byte)(1 << i);
//        else
//            Mask &= (byte)~(1 << i);
//    }
//    public void ClearMask()
//    {
//        Mask = 0;
//    }
//    public static implicit operator byte(DirtyByteMask m)
//    {
//        return m.Mask;
//    }
//    public static explicit operator DirtyByteMask(byte m)
//    {
//        return new DirtyByteMask() { Mask = m };
//    }
//}