using MobaGame.Framework.Core.Modules;
using MobaGame.Framework.Core.Modules.Ability;
using System;
using System.Collections.Generic;
using MobaGame.Framework.Core;
using MobaGame.Framework.Core.Modules.Ability.Helpers;
using MobaGame.Framework.Types;
using UnityEngine;

namespace MobaGame.Entity.Abilities.ForestGuardian
{
	[CreateAssetMenu(menuName = "Ability/ForestGuardian/Black Rose Curse")]
	public class BlackRoseCurse : AbilityObject, IListener<IStepableEvent>
	{
		[SerializeField]
		private Sprite _rrIcon;
		[SerializeField]
		private Sprite _brIcon;
		[SerializeField] private float _cooldown;
		[SerializeField] private float _manaCost;
		
		public SimpleAbilityView View { get; set; }

		public AbilityPredicateBuilder CheckBuilder { get; set; }
		public override void Initialize(Actor data)
		{
			base.Initialize(data);
			CheckBuilder = new AbilityPredicateBuilder(data)
			{
				Cooldown = new Cooldown(_cooldown),
				MagicCost = new MagicCost(Modules.Magicable, _manaCost),
			};
			View = new SimpleAbilityView()
			{
				Icon = _rrIcon,
				Cooldown = CheckBuilder.Cooldown,
				StatCost = CheckBuilder.MagicCost,
				Toggleable = new ToggleableAbilityView()
				{
					Active = false,
					ShowActive = false
				}
			};
			CheckBuilder.RebuildChecks();
			View.Toggleable.Toggled += ToggleableOnToggled;
			Register(data);
		}

		public override IAbilityView GetAbilityView() => View;

		public bool Active => View.Toggleable.Active;

		public event EventHandler<ChangedEventArgs<bool>> Toggled
		{
			add => View.Toggleable.Toggled += value;
			remove => View.Toggleable.Toggled -= value;
		}

		public override void ConfirmCast()
		{
			if(!CheckBuilder.AllowCast())
				return;
			View.Toggleable.Toggle();
			CheckBuilder.DoCast();
			var args = new AbilityEventArgs(Self,View.StatCost.Cost);
			Modules.Abilitiable.NotifyAbilityCast(args);
			
		}

		private void ToggleableOnToggled(object sender, ChangedEventArgs<bool> e)
		{
			var isBlackRose = e.After;
			View.Icon = isBlackRose ? _brIcon : _rrIcon;
		}

		public void Register(IStepableEvent source)
		{
			CheckBuilder.Cooldown.Register(source);
		}


		public void Unregister(IStepableEvent source)
		{
			CheckBuilder.Cooldown.Unregister(source);
		}
	}
}