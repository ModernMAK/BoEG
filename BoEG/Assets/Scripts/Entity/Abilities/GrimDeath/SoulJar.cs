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
	public class SoulJar : AbilityObject, IListener<IStepableEvent>
	{
		#pragma warning disable 0649
		[SerializeField]
		private int _soulsAcquired;
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

		public float HealthGen => _healthGenPerSoul * _soulsAcquired;
		public float ManaGen => _manaGenPerSoul * _manaGenPerSoul;

	

		public override void Initialize(Actor actor)
		{
			base.Initialize(actor);
			Register(actor);
			_aura = TriggerUtility.CreateTrigger<SphereCollider>(actor, "SoulJar Search Trigger");
			_aura.Collider.radius = _searchRadius;
			_aura.Trigger.Enter += OnEnter;
			_aura.Trigger.Exit += OnExit;
			Modules.Healthable.Died += OnDeath;
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

		private void OnPreStep(float step)
		{
			Modules.Healthable.Health += step * HealthGen;
			Modules.Magicable.Magic += step * ManaGen;
		}
		public void Register(IStepableEvent source)
		{
			source.PreStep += OnPreStep;
		}

		public void Unregister(IStepableEvent source)
		{
			source.PreStep -= OnPreStep;
		}


		private void AddSouls(int souls)
		{
			_soulsAcquired += souls;
			_soulsAcquired = Mathf.Clamp(_soulsAcquired, 0, _soulLimit);
		}
		private void HalveSouls()
		{
			_soulsAcquired /= 2;
		}

	}
}