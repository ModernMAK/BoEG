using System;
using System.Collections;
using System.Collections.Generic;
using Framework.Core.Modules;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    private IAttackerable _attackerable;

    private void Awake()
    {
        _attackerable = GetComponent<IAttackerable>();
        enabled = (_attackerable != null);
    }

    // Update is called once per frame
    void Update()
    {
            if (_attackerable.HasAttackTarget() && !_attackerable.IsAttackOnCooldown)
                _attackerable.Attack(_attackerable.GetAttackTarget(0));
    }
}