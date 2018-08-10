namespace Old.Entity.Core
{
    public interface IDataGroup
    {
        T GetData<T>();
    }
}