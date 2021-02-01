using MobaGame.Framework.Core;
using MobaGame.Framework.Core.Modules;
using MobaGame.Framework.Core.Modules.Ability;
using MobaGame.Framework.Core.Trigger;
using System;
using UnityEngine;

namespace MobaGame.Entity.Abilities.GrimDeath
{

	[CreateAssetMenu(menuName = "Ability/GrimDeath/SoulJar")]
	public class SoulJar : AbilityObject
	{
#pragma warning disable 0649
		[SerializeField]
		private int _soulsPerKill;
		[SerializeField]
		private int _soulsPerAssist;

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

			public FloatModifier HealthGeneration => new FloatModifier(HealthGenerationBonus);

			public FloatModifier MagicGeneration => new FloatModifier(MagicGenerationBonus);

			public event EventHandler Changed;
			private void OnChanged() => Changed?.Invoke(this, EventArgs.Empty);
		}

		SoulJarModifier _modifier;
		public override void Initialize(Actor data)
		{
			_modifier = new SoulJarModifier(_healthGenPerSoul,_manaGenPerSoul);
			base.Initialize(data);
			_aura = TriggerUtility.CreateTrigger<SphereCollider>(data, "SoulJar Search Trigger");
			_aura.Collider.radius = _searchRadius;
			_aura.Trigger.Enter += OnEnter;
			_aura.Trigger.Exit += OnExit;
			Modules.Killable.Died += OnDeath;
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
			if (!actor.TryGetModule<IKillable>(out var killable))
				return;

			killable.Died -= OnTargetDeath;
		}

		private void OnEnter(object sender, TriggerEventArgs e)
		{
			var collider = e.Collider;
			if (!AbilityHelper.TryGetActor(collider, out var actor))
				return;
			if (!actor.TryGetModule<IKillable>(out var killable))
				return;

			killable.Died += OnTargetDeath;
		}

		private void OnTargetDeath(object sender, DeathEventArgs e)
		{
			var killable = sender as IKillable;
			killable.Died -= OnTargetDeath;
			if (e.Killer == Self)
				AddSouls(_soulsPerKill);
			else
				AddSouls(_soulsPerAssist);
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