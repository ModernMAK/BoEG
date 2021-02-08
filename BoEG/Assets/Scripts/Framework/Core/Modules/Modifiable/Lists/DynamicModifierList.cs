using System;

namespace MobaGame.Framework.Core
{
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
}