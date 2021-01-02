using System;
using System.Collections;
using System.Collections.Generic;
using Entity.Abilities.FlameWitch;
using Framework.Ability;
using Framework.Core.Modules;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class AbilityPanel : DebugUI
{
    private IAbilityView _view;
    private IMagicable _magicable;

    public override void SetTarget(GameObject go)
    {
        _magicable = go.GetComponent<IMagicable>();
    }

    public void SetAbility(IAbility ability)
    {
        _view = ability.GetAbilityView();
        _iconFX.sprite = _icon.sprite = _view.GetIcon();
    }

    private void Update()
    {
        
        UpdateImageFill(1f-_view.GetCooldownProgress(), _iconFX, 3);
    }


#pragma warning disable 649
    [SerializeField] private Image _icon;
    [SerializeField] private Image _iconFX;

#pragma warning restore 649
}