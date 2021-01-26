using MobaGame.Framework.Core.Modules;
using System;
using System.Collections;
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


	public class DynamicModifierList<T> : ModifierList<T> where T : IDynamicModifier
	{

		public EventHandler<T> ModifierChanged;
		protected override void AddModifier(T modifier)
		{
			base.AddModifier(modifier);
			var dynamic = modifier;
			dynamic.Changed += OnModifierChanged;
		}
		protected override void RemoveModifier(T modifier)
		{
			base.RemoveModifier(modifier);
			var dynamic = modifier;
			dynamic.Changed -= OnModifierChanged;
		}

		private void OnModifierChanged(object sender, EventArgs e)
		{
			var modifier = (T)sender;
			ModifierChanged?.Invoke(this, modifier);
			OnListChanged();
		}
	}
	public class MixedModifierList<T> : ModifierList<T> where T : IModifier
	{

		public EventHandler<T> ModifierChanged;
		protected override void AddModifier(T modifier)
		{
			base.AddModifier(modifier);
			if(modifier is IDynamicModifier dynamic)
				dynamic.Changed += OnModifierChanged;
		}
		protected override void RemoveModifier(T modifier)
		{
			base.RemoveModifier(modifier);
			if (modifier is IDynamicModifier dynamic)
				dynamic.Changed -= OnModifierChanged;
		}

		private void OnModifierChanged(object sender, EventArgs e)
		{
			var modifier = (T)sender;
			ModifierChanged?.Invoke(this, modifier);
			OnListChanged();
		}
	}
	public class ModifierList<T> : IListener<IModifiable>, IReadOnlyList<T> where T : IModifier
	{
		public ModifierList()
		{
			_modifiers = new List<T>();
		}
		private List<T> _modifiers;

		public void Register(IModifiable source)
		{
			source.OnModifierAdded += OnModifierAdded;
		}
		public void Unregister(IModifiable source)
		{
			source.OnModifierRemoved += OnModifierRemoved;
		}
		   
		public event EventHandler<T> ModifierAdded;
		public event EventHandler<T> ModifierRemoved;
		public event EventHandler ListChanged;

		protected void OnListChanged() => ListChanged?.Invoke(this, EventArgs.Empty);
		private void OnModifierAdded(object sender, IModifier e)
		{
			if (e is T mod)
			{
				AddModifier(mod);
				ModifierAdded?.Invoke(sender, mod);
				OnListChanged();
			}
		}
		private void OnModifierRemoved(object sender, IModifier e)
		{
			if (e is T mod)
			{
				RemoveModifier(mod);
				ModifierRemoved?.Invoke(sender, mod);
				OnListChanged();
				
			}
		}
		protected virtual void AddModifier(T modifier)
		{
			_modifiers.Add(modifier);
		}
		protected virtual void RemoveModifier(T modifier)
		{
			_modifiers.Remove(modifier);
		}

		public int Count => _modifiers.Count;
		public T this[int index] => _modifiers[index];

		public IEnumerator<T> GetEnumerator()
		{
			return _modifiers.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _modifiers.GetEnumerator();
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