using System;
using System.Collections;
using System.Collections.Generic;
using Framework.Core.Modules;
using Commands = Framework.Core.Modules.Commands;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    private ICommandable _commandable;

    private void Awake()
    {
        _commandable = GetComponent<ICommandable>();
    }

    //V1
    //We want to attack move towards a position, until we die
    public void SetAttackTarget(Vector3 position)
    {
        //TODO
        var attackMoveCommand = new Commands.AttackMoveCommand(gameObject, position);

        _commandable.InterruptCommand(default);
    }
}