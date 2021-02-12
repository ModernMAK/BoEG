using MobaGame.Framework.Types;

namespace MobaGame.Framework.Core.Modules.Ability.Helpers
{
	/// <summary>
	/// A class for two cooldowns to share a 'view'.
	/// </summary>
	/// <remarks>
	/// Expected to be used to abilities that are two seperate abilities and do not share a cooldown.
	/// If they share a cooldown, a single Cooldown can be used.
	/// </remarks>
	public class DualCooldown : CooldownBase
	{
		public DualCooldown(float active, float inactive, bool updateBoth = true) : this(new DurationTimer(active, true), new DurationTimer(inactive,true), updateBoth) { }

		public DualCooldown(DurationTimer inactive, DurationTimer active, bool updateBoth = true)
		{
			Timer = inactive;
			InactiveTimer = inactive;
			ActiveTimer = active;
			UpdateBoth = updateBoth;
		}
		public DurationTimer ActiveTimer { get; }
		public DurationTimer InactiveTimer { get; }
		public bool UpdateBoth { get; }
		public void SetTimer(bool active)
		{
			var timer = active ? ActiveTimer : InactiveTimer;
			Timer = timer;			
		}
		protected override void StepTimers(float deltaTime)
		{
			if (!UpdateBoth)
			{
				base.StepTimers(deltaTime);
			}
			else
			{
				ActiveTimer.AdvanceTimeIfNotDone(deltaTime);
				InactiveTimer.AdvanceTimeIfNotDone(deltaTime);
			}
		}

	}
}