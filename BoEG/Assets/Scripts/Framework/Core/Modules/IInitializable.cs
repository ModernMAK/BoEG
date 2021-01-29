namespace MobaGame.Framework.Core.Modules
{
    /// <summary>
    /// Allows an object to be initialized after it's been created.
    /// </summary>
    /// <typeparam name="TData">
    /// The type of data to pass in.
    /// </typeparam>
    public interface IInitializable<in TData>
    {
        /// <summary>
        /// Initialize the object with the given data.
        /// </summary>
        /// <param name="data"></param>
        void Initialize(TData data);
    }
}