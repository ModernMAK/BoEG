namespace MobaGame.Framework.Core.Modules
{
	/// <summary>
	/// Represents a Generically typed modified value.
	/// This interface is Read Only.
	/// </summary>
	/// <typeparam name="T">The type of modifier.</typeparam>
	public interface IModifiedValue<out T>
	{
		/// <summary>
		/// The value before modifiers are applied.
		/// </summary>
        public T Base { get; }
		/// <summary>
		/// The value to add, calculated from modifiers.
		/// </summary>
        public T Bonus { get; }
		/// <summary>
		/// The value after modifiers are applied.
		/// </summary>
        public T Total { get; }
    }

}