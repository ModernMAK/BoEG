using System;
using System.Collections;
using System.Collections.Generic;
using Framework.Core;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class IconPanel : DebugUI
{
#pragma warning disable 0649

    [SerializeField] private Image _icon;
#pragma warning restore 0649

    public override void SetTarget(GameObject go)
    {
        if (go != null)
            _actor = go.GetComponent<Actor>();
        else
            _actor = null;
    }

    private void Update()
    {
        if (_icon != null)
        {
            _icon.sprite = _actor != null ? _actor.GetIcon() : null;
        }
    }


    private Actor _actor;
}