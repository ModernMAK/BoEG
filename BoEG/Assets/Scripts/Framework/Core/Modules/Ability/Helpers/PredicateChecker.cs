using System;

namespace MobaGame.Framework.Core.Modules.Ability.Helpers
{
	/// <remarks>
	/// I made this because im a nerd whose making assumptions about managed memory;
	/// Originally i was just going to use an internal funciton to handle caching a predicate list and merging them into one predicate, 
	/// But sinceI plan to use this alot, for abilities, I decided to make this, hopefully it's better for memory, but maybe it was pointless.
	/// </remarks>
	public class PredicateChecker<TArg>
	{
		public PredicateChecker(params Func<TArg, bool>[] predicates)
		{
			Predicates = predicates;
		}
		private Func<TArg, bool>[] Predicates { get; }

		public bool Evaluate(TArg value)
		{
			foreach (var predicate in Predicates)
				if (!predicate(value))
					return false;
			return true;
		}
	}
	public class PredicateChecker
	{

		public PredicateChecker(params Func<bool>[] predicates)
		{
			Predicates = predicates;
		}
		private Func<bool>[] Predicates { get; }

		public bool Evaluate()
		{
			foreach (var predicate in Predicates)
				if (!predicate())
					return false;
			return true;
		}
	}
}