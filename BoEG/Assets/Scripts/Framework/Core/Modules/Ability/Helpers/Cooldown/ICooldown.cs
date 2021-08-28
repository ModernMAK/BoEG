namespace MobaGame.Framework.Core.Modules.Ability.Helpers
{
	public interface ICooldown
	{
		float Duration { get; }
		float Normal { get; }
		float Remaining { get; }
		float Elapsed { get; }
		bool IsDone { get; }
	}
}