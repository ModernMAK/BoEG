﻿using MobaGame.Framework.Core;
using MobaGame.Framework.Core.Modules;
using MobaGame.Framework.Core.Modules.Ability;
using MobaGame.Framework.Core.Trigger;
using System;
using MobaGame.Framework.Utility;
using UnityEngine;

namespace MobaGame.Entity.Abilities.GrimDeath
{

	[CreateAssetMenu(menuName = "Ability/GrimDeath/SoulJar")]
	public class SoulJar : AbilityObject
	{
		[SerializeField] private Sprite _icon;
#pragma warning disable 0649
		[SerializeField]
		private int _soulsPerKill;
		[SerializeField]
		private int _soulsPerAssist;

		[Header("Modifier")] [SerializeField] 
		private Sprite _modifierIcon;
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

		private SoulJarModifier _modifier;
		public SoulJarModifier Modifier => _modifier;

		public class SoulJarModifier : IHealthGenerationModifier, IMagicGenerationModifier, IDynamicModifier
		{
			public SoulJarModifier(Sprite icon, float hpGen, float mpGen, int souls=default)
			{
				_souls = souls;
				_hpGenPerSoul = hpGen;
				_mpGenPerSoul = mpGen;
				_view = new ModifierView()
				{
					Icon = icon
				};
			}
			private int _souls;
			private float _hpGenPerSoul;
			private float _mpGenPerSoul;
			private readonly ModifierView _view;
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
					var changed = !_hpGenPerSoul.SafeEquals(value);
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
					var changed = !_mpGenPerSoul.SafeEquals(value);
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

			public IModifierView View => _view;
		}

		public override void Initialize(Actor data)
		{
			_modifier = new SoulJarModifier(_modifierIcon,_healthGenPerSoul,_manaGenPerSoul);
			base.Initialize(data);
			base.Initialize(data);
			View = new SimpleAbilityView()
			{
				Icon = _icon
			};
			_aura = TriggerUtility.CreateTrigger<SphereCollider>(data, "SoulJar Search Trigger");
			_aura.Collider.radius = _searchRadius;
			_aura.Trigger.Enter += OnEnter;
			_aura.Trigger.Exit += OnExit;
			Modules.Killable.Died += OnDeath;
			Modules.Modifiable.AddModifier(_modifier);
		}

		public SimpleAbilityView View { get; set; }
		public override IAbilityView GetAbilityView() => View;
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