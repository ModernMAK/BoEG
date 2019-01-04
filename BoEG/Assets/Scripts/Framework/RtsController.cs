using System.Collections;
using System.Collections.Generic;
using Framework.Core;
using Framework.Core.Modules;
using Framework.Core.Modules.Commands;
using UnityEngine;

public class RtsController : MonoBehaviour
{
    [SerializeField] private Commandable _commandable;

    private KeyCode _moveKey = KeyCode.M;
    private int _actionKey = 1;
    private int _selectKey = 0;
    private KeyCode _followKey = KeyCode.F;
    private KeyCode _queueKey = KeyCode.LeftShift;

    private Camera _main;

    private Camera CameraMain
    {
        get
        {
            if (_main == null)
                _main = Camera.main;
            return _main;
        }
    }

    private Ray CameraRay
    {
        get { return CameraMain.ScreenPointToRay(Input.mousePosition); }
    }

    bool PerformCast(out RaycastHit info)
    {
        return Physics.Raycast(CameraRay, out info);
    }

    void Update()
    {
        if (_commandable != null && Input.GetMouseButton(_actionKey))
        {
            RaycastHit info;
            if (PerformCast(out info))
            {
                var point = info.point;
                var unit = info.collider.GetComponentInParent<Unit>();


                if (unit != null && !Input.GetKey(_moveKey))
                    AddOrQueueCommand(GenerateFollow(unit.transform));
                else if (!Input.GetKey(_followKey))
                    AddOrQueueCommand(GenerateMove(point));
            }
        }
        if (Input.GetMouseButton(_selectKey))
        {
            RaycastHit info;
            if (PerformCast(out info))
            {
                var commandable = info.collider.GetComponentInParent<Commandable>();
                if (commandable != null)
                    _commandable = commandable;
            }
        }
    }

    FollowCommand GenerateFollow(Transform target)
    {
        var movable = _commandable.GetComponent<IMovable>();
        return new FollowCommand(movable, _commandable.transform, target);
    }

    MoveToCommand GenerateMove(Vector3 target)
    {
        var movable = _commandable.GetComponent<IMovable>();
        return new MoveToCommand(movable, target);
    }

    void AddOrQueueCommand(ICommand command)
    {
        if (Input.GetKey(_queueKey))
            _commandable.AddCommand(command);
        else
            _commandable.SetCommand(command);
    }
}