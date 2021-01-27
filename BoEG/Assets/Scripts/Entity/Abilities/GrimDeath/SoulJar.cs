using Framework.Core;
using MobaGame.Framework.Core;
using MobaGame.Framework.Core.Modules;
using MobaGame.Framework.Core.Modules.Ability;
using MobaGame.Framework.Core.Trigger;
using MobaGame.Framework.Types;
using System;
using UnityEngine;

namespace MobaGame.Entity.Abilities.GrimDeath
{
	public class SoulJar : AbilityObject
	{
		#pragma warning disable 0649
		[SerializeField]
		private int _soulLimit;
		[SerializeField]
		private float _healthGenPerSoul;
		[SerializeField]
		private float _manaGenPerSoul;
		[SerializeField]
		private float _searchRadius;
#pragma warning restore 0649
		private TriggerHelper<SphereCollider> _aura;


		//TODO add magicable modifier
		private class SoulJarModifier : IHealthGenerationModifier, IMagicGenerationModifier, IDynamicModifier
		{
			public SoulJarModifier(float hpGen, float mpGen, int souls=default)
			{
				_souls = souls;
				_hpGenPerSoul = hpGen;
				_mpGenPerSoul = mpGen;
			}
			private int _souls;
			private float _hpGenPerSoul;
			private float _mpGenPerSoul;
			public int Souls
			{
				get => _souls;
				set
				{
					var changed = _souls != value;
					_souls = value;
					if (changed)
						OnChanged();
				}
			}
			public float HealthGenPerSoul
			{
				get => _hpGenPerSoul;
				set
				{
					var changed = _hpGenPerSoul != value;
					_hpGenPerSoul = value;
					if (changed)
						OnChanged();
				}
			}
			public float ManaGenPerSoul
			{
				get => _mpGenPerSoul;
				set
				{
					var changed = _mpGenPerSoul != value;
					_mpGenPerSoul = value;
					if (changed)
						OnChanged();
				}
			}


			public float HealthGenerationBonus => HealthGenPerSoul * Souls;
			public float MagicGenerationBonus => ManaGenPerSoul * Souls;

			public Modifier HealthGeneration => new Modifier(HealthGenerationBonus);

			public Modifier MagicGeneration => new Modifier(MagicGenerationBonus);

			public event EventHandler Changed;
			private void OnChanged() => Changed?.Invoke(this, EventArgs.Empty);
		}

		SoulJarModifier _modifier;
		public override void Initialize(Actor actor)
		{
			_modifier = new SoulJarModifier(_healthGenPerSoul,_manaGenPerSoul);
			base.Initialize(actor);
			_aura = TriggerUtility.CreateTrigger<SphereCollider>(actor, "SoulJar Search Trigger");
			_aura.Collider.radius = _searchRadius;
			_aura.Trigger.Enter += OnEnter;
			_aura.Trigger.Exit += OnExit;
			Modules.Healthable.Died += OnDeath;
			Modules.Modifiable.AddModifier(_modifier);
		}

		private void OnDeath(object sender, DeathEventArgs e)
		{
			HalveSouls();
		}

		private void OnExit(object sender, TriggerEventArgs e)
		{
			var collider = e.Collider;
			if (!AbilityHelper.TryGetActor(collider, out var actor))
				return;
			if (!actor.TryGetModule<IHealthable>(out var healthable))
				return;

			healthable.Died -= OnTargetDeath;
		}

		private void OnEnter(object sender, TriggerEventArgs e)
		{
			var collider = e.Collider;
			if (!AbilityHelper.TryGetActor(collider, out var actor))
				return;
			if (!actor.TryGetModule<IHealthable>(out var healthable))
				return;

			healthable.Died += OnTargetDeath;
		}

		private void OnTargetDeath(object sender, DeathEventArgs e)
		{
			var healthable = sender as IHealthable;
			healthable.Died -= OnTargetDeath;
			AddSouls(1);
		}



		private void AddSouls(int souls)
		{
			_modifier.Souls = Mathf.Clamp(_modifier.Souls + souls, 0, _soulLimit);
		}
		private void HalveSouls()
		{
			_modifier.Souls /= 2;
		}

	}
}