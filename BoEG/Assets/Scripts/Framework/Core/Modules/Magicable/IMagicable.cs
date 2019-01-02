namespace Framework.Core.Modules
{
    public interface IMagicable
    {
        PointData Magic { get; }
        void ModifyMagic(float deltaValue);
        event MagicChangeHandler MagicModifying;
        event MagicChangeHandler MagicModified;
    }
}