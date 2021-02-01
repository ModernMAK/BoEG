using MobaGame.Framework.Types;

namespace MobaGame.Framework.Core.Modules
{
	public class CritEffect : IListener<IAttackerable>
	{
		private readonly RandomProc _proc;
		private readonly float _critMultiplier;
		public CritEffect(RandomProc proc, float critMultiplier)
		{
			_proc = proc;
			_critMultiplier = critMultiplier;
		}
		private void CritProc(object sender, AttackCritEventArgs e)
		{
			if (e.CriticalMultiplier > _critMultiplier)
				return;

			if (_proc.Proc())
			{
				e.CriticalMultiplier = _critMultiplier;
			}
		}
		public void Register(IAttackerable source)
		{
			source.CritModifiers += CritProc;
		}

		public void Unregister(IAttackerable source)
		{
			source.CritModifiers -= CritProc;
		}
	}
	public class CritItem : ActorItem
	{
		public CritItem(CritEffect crit)
		{
			_crit = crit;
		}
		private readonly CritEffect _crit;

		public override void Register(Actor source)
		{
			if (source.TryGetModule<IAttackerable>(out var attackerable))
				_crit.Register(attackerable);
		}

		public override void Unregister(Actor source)
		{
			if (source.TryGetModule<IAttackerable>(out var attackerable))
				_crit.Unregister(attackerable);
		}
	}
	public class GenerationModifierItemObject : ScriptableItem, IHealthGenerationModifier, IMagicGenerationModifier, IListener<Actor>, IListener<IModifiable>
	{
		private float _manaGen;

		void Awake()
		{
			var healthMod = new FloatModifier();
			var magicMod = new FloatModifier();
			_item = new GenerationModifierItem(healthMod, magicMod);
		}
		private GenerationModifierItem _item;
		public FloatModifier HealthGeneration => _item.HealthGeneration;

		public FloatModifier MagicGeneration => _item.MagicGeneration;
	}
	public class GenerationModifierItem : ModifierItem, IHealthGenerationModifier, IMagicGenerationModifier
	{
		public GenerationModifierItem(FloatModifier healthGen, FloatModifier manaGen)
		{
			HealthGeneration = healthGen;
			MagicGeneration = manaGen;
		}
		public FloatModifier HealthGeneration { get; }

		public FloatModifier MagicGeneration { get; }

		public override void Register(IModifiable source) => source.AddModifier(this);

		public override void Unregister(IModifiable source) => source.RemoveModifier(this);
	}
}