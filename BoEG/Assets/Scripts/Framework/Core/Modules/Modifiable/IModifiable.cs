using MobaGame.Framework.Core.Modules;
using System;
using System.Collections.Generic;

namespace MobaGame.Framework.Core
{
	public class Modifiable : ActorModule, IModifiable
	{
		private List<IModifier> _modifiers;

		public Modifiable(Actor actor) : base(actor)
		{
			_modifiers = new List<IModifier>();
		}

		public IReadOnlyList<IModifier> Modifiers => _modifiers;
		public event EventHandler<IModifier> OnModifierAdded;
		public event EventHandler<IModifier> OnModifierRemoved;

		public void AddModifier(IModifier modifier)
		{
			_modifiers.Add(modifier);
			OnModifierAdded?.Invoke(this, modifier);
		}

		public void RemoveModifier(IModifier modifier)
		{
			if(_modifiers.Remove(modifier))
				OnModifierRemoved?.Invoke(this, modifier);
		}
	}

	public interface IModifiable
    {
        event EventHandler<IModifier> OnModifierAdded;
        event EventHandler<IModifier> OnModifierRemoved;
        void AddModifier(IModifier modifier);
        void RemoveModifier(IModifier modifier);
        IReadOnlyList<IModifier> Modifiers { get; }
    }
	public static class IModifiableX
	{
		public static IEnumerable<T> GetModifiers<T>(this IModifiable modifiable) => EnumerableQuery.GetAll<T>(modifiable.Modifiers);

	}
}