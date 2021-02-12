using MobaGame.Framework.Core.Modules;
using MobaGame.Framework.Core.Modules.Ability;
using System;
using System.Collections.Generic;
using MobaGame.Framework.Core;
using MobaGame.Framework.Types;
using UnityEngine;

namespace MobaGame.Entity.Abilities.ForestGuardian
{
	[CreateAssetMenu(menuName = "Ability/ForestGuardian/Black Rose Curse")]
	public class BlackRoseCurse : AbilityObject, IListener<IStepableEvent>
	{
		private Sprite _rrIcon => _icon;
		[SerializeField]
		private Sprite _brIcon;
		[SerializeField] private float _cooldown;
		[SerializeField] private float _manaCost;
		
		private SimpleAbilityView _view;
		public override void Initialize(Actor data)
		{
			base.Initialize(data);
			_view = new SimpleAbilityView()
			{
				Icon = _rrIcon,
				Cooldown = new CooldownAbilityView(new DurationTimer(_cooldown,true)),
				StatCost = new MagicCost(Modules.Magicable, _manaCost),
				Toggleable = new ToggleableAbilityView(this)
				{
					ShowActive = false
				}
			};
			_view.Toggleable.Toggled += ToggleableOnToggled;
			Register(data);
		}

		public override IAbilityView GetAbilityView() => _view;

		public bool Active => _view.Toggleable.Active;

		public event EventHandler<ChangedEventArgs<bool>> Toggled
		{
			add => _view.Toggleable.Toggled += value;
			remove => _view.Toggleable.Toggled -= value;
		}

		public override void ConfirmCast()
		{
			if(_view.Cooldown.OnCooldown)
				return;
			if(!_view.StatCost.TrySpendCost())
				return;
			_view.Toggleable.Toggle();
			_view.Cooldown.StartCooldown();
			var args = new AbilityEventArgs(Self,_view.StatCost.Cost);
			Modules.Abilitiable.NotifyAbilityCast(args);
			
		}

		private void ToggleableOnToggled(object sender, ChangedEventArgs<bool> e)
		{
			var isBlackRose = e.After;
			_view.Icon = isBlackRose ? _brIcon : _rrIcon;
		}

		public void Register(IStepableEvent source)
		{
			source.PreStep += OnPreStep;
		}

		private void OnPreStep(float deltaTime)
		{
			_view.Cooldown.AdvanceTime(deltaTime);
		}

		public void Unregister(IStepableEvent source)
		{
			source.PreStep-= OnPreStep;
		}
	}
}