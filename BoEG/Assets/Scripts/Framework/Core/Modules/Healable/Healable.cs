using MobaGame.Framework.Types;
using System;

namespace MobaGame.Framework.Core.Modules
{
	public class Healable : IHealable
	{
		private readonly IHealthable _healthable;
		public Healable(IHealthable healthable)
		{
			_healthable = healthable;
		}

		public event EventHandler<SourcedHeal> Healing;
		public event EventHandler<SourcedHeal> Healed;
		public event EventHandler<ChangableEventArgs<SourcedHeal>> HealingModifiers;

		public void Heal(SourcedHeal heal)
		{
			heal = CalculateHealingModifiers(heal);
			OnHealing(heal);
			_healthable.Value += heal.Value;
			OnHealed(heal);
		}

		protected virtual void OnHealing(SourcedHeal heal) => Healing?.Invoke(this, heal);
		protected virtual void OnHealed(SourcedHeal heal) => Healed?.Invoke(this, heal);
		protected virtual SourcedHeal CalculateHealingModifiers(SourcedHeal heal) => HealingModifiers.CalculateChange(this, heal);
	}
}