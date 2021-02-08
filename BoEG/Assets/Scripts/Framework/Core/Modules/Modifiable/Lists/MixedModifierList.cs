using System;

namespace MobaGame.Framework.Core
{
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
}