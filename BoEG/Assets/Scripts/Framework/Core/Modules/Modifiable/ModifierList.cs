using System;
using System.Collections;
using System.Collections.Generic;

namespace MobaGame.Framework.Core
{
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
}