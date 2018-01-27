using UnityEngine;
using UnityEngine.Networking;

public class Healthable : RegenStatable
{
    [SerializeField] private bool _isDead;

    public bool IsDead
    {
        get { return _isDead; }
        private set
        {
            if (_isDead != value)
                SetDirtyBit(7);
            _isDead = value;
        }
    }

    protected override void Update()
    {
        if (!isServer) return;
        if (IsDead) return;
        Generate(TotalGenerated);
        if (ValueNormalized == 0f)
            IsDead = true;
    }

    protected override void ModuleSerialize(ModuleWriter writer)
    {
        base.ModuleSerialize(writer);

        writer.WriteIf(IsDead, 7);
    }

    protected override void ModuleDeserialize(NetworkReader reader, uint mask, bool readMask = false)
    {
        if (readMask)
            mask = reader.ReadUInt32();

        if (mask.GetBit(7))
            _isDead = reader.ReadBoolean();

        base.ModuleDeserialize(reader, mask);
    }
}