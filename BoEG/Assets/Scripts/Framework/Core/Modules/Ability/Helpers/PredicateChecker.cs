using MobaGame.Assets.Scripts.Framework.Core.Modules.Ability.Helpers;
using System;
using System.Collections.Generic;

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

	public class AbilityPredicateBuilder
	{
		public AbilityPredicateBuilder(Actor self)
		{
			Self = self;
			TargetCheck = new PredicateChecker<Actor>();
			CastingCheck = new PredicateChecker();
		}
		private Actor Self { get; }
		public bool AllowSelf { get; set; }
		public TeamableChecker Teamable { get; set; }
		public Cooldown Cooldown { get; set; }
		public CastRange CastRange { get; set; }

		/// <summary>
		/// Checks to be used on a per-target basis
		/// </summary>
		public PredicateChecker<Actor> TargetCheck { get; private set; }
		/// <summary>
		/// Checks to be used on a per-cast basis
		/// </summary>
		public PredicateChecker CastingCheck { get; private set; }

		public bool AllowTarget(Actor actor) => TargetCheck.Evaluate(actor);
		public bool AllowCast() => CastingCheck.Evaluate();


		private void RebuildActorCheck()
		{
			var preds = new List<Func<Actor,bool>>();
			if (AllowSelf)
			{
				var p = SimpleAbilityPreciates.IsSelf(Self);
				preds.Add(p);
			}
			if (CastRange != null)
				preds.Add(CastRange.InRange);
			if (Teamable != null)
				preds.Add(Teamable.IsAllowed);
			TargetCheck = new PredicateChecker<Actor>(preds.ToArray());
		}
		private void RebuildUniversalCheck()
		{
			var preds = new List<Func<bool>>();
			if (Cooldown != null) 
			{
				var p = SimpleAbilityPreciates.IsDone(Cooldown);
				preds.Add(p);
			}
		}
		public void RebuildChecks()
		{
			RebuildUniversalCheck();
			RebuildActorCheck();
		}
	}
}