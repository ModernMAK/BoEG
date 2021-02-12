using MobaGame.Framework.Types;
using System;

namespace MobaGame.Framework.Core.Modules.Ability.Helpers
{
	public class CooldownBase : ICooldownView, IListener<IStepableEvent>
	{
		private DurationTimer _timer;
		public DurationTimer Timer { 
			get => _timer;
			protected set
			{
				if (_timer != null)
					_timer.Changed -= InternalOnChanged;
				_timer = value;
				if (_timer != null)
					_timer.Changed += InternalOnChanged;
			}
		}

		public void Begin() => Timer.ElapsedTime = 0;
		public void End() => Timer.ElapsedTime = Timer.Duration;

		public float Duration => Timer.Duration;
		public float Normal => Timer.ElapsedTime / Timer.Duration;
		public float Elapsed => Timer.ElapsedTime;
		public float Remaining => Timer.RemainingTime;
		public bool IsDone => Timer.Done;
		public void InternalOnChanged(object sender, EventArgs e) => OnChanged();

		public event EventHandler Changed;
		private void OnChanged() => Changed?.Invoke(this, EventArgs.Empty);
		public void Register(IStepableEvent source) => source.PreStep += StepTimers;
		public void Unregister(IStepableEvent source) => source.PreStep -= StepTimers;
		protected virtual void StepTimers(float deltaTime) => Timer.AdvanceTimeIfNotDone(deltaTime);
		public void Advance(float deltaTime) => StepTimers(deltaTime);
	}
}