using System;
using System.Collections.Generic;

namespace MobaGame.Framework.Core.Modules.Ability.Helpers
{
	public class AbilityPredicateBuilder
	{
		public AbilityPredicateBuilder(Actor self)
		{
			Self = self;
			TargetCheck = new PredicateChecker<Actor>();
			CastingCheck = new PredicateChecker();
			CastActions = new Action[0];
		}
		private Actor Self { get; }
		public bool AllowSelf { get; set; }
		public TeamableChecker Teamable { get; set; }
		public CooldownBase Cooldown { get; set; }
		public CastRange CastRange { get; set; }
		public MagicCost MagicCost { get; set; }
		/// <summary>
		/// Checks to be used on a per-target basis
		/// </summary>
		private PredicateChecker<Actor> TargetCheck { get; set; }
		/// <summary>
		/// Checks to be used on a per-cast basis
		/// </summary>
		private PredicateChecker CastingCheck { get; set; }
		private Action[] CastActions { get; set; }



		public bool AllowTarget(Actor actor) => TargetCheck.Evaluate(actor);
		public bool AllowCast() => CastingCheck.Evaluate();
		public void DoCast()
		{
			foreach (var act in CastActions)
				act();
		}

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
			if (MagicCost != null)
				preds.Add(MagicCost.CanSpendCost);
			CastingCheck = new PredicateChecker(preds.ToArray());
		}
		private void RebuildCastActions()
		{
			var preds = new List<Action>();
			if (Cooldown != null)
				preds.Add(Cooldown.Begin);
			if (MagicCost != null)
				preds.Add(Cooldown.Begin);
			CastActions = preds.ToArray();

		}
		public void RebuildChecks()
		{
			RebuildUniversalCheck();
			RebuildActorCheck();
			RebuildCastActions();
		}
	}
}