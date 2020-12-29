using System;
using System.Collections;
using System.Collections.Generic;
using Framework.Core.Modules;
using UI;
using UnityEngine;

public class AttackRangeDebugFX : DebugUI
{
    private IAttackerable _attackerable;

    // Start is called before the first frame update
    private GameObject _go;

    public override void SetTarget(GameObject go)
    {
        _go = go;
        _attackerable = _go != null ? _go.GetComponent<IAttackerable>() : null;
        transform.LookAt(Vector3.up);
    }

    // Update is called once per frame
    private void Update()
    {
        var attackRange = 0f;    
        if (_attackerable != null)
        {
            attackRange = _attackerable.AttackRange;
        }

        _renderer.SetRadius(attackRange, true);
    }

#pragma warning disable 649
    [SerializeField] private RingRenderer _renderer;
#pragma warning restore 649

    void Awake()
    {
        if (_renderer == null)
            _renderer = GetComponent<RingRenderer>();
    }
}