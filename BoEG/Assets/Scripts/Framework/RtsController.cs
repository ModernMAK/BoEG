using Framework.Core;
using Framework.Core.Modules;
using Framework.Core.Modules.Commands;
using UnityEngine;

public class RtsController : MonoBehaviour
{
    [SerializeField] private CommandableComponent _commandable;

    private Camera _main;

    private const KeyCode _followKey = KeyCode.F;
    private const KeyCode _moveKey = KeyCode.M;
    private const KeyCode _queueKey = KeyCode.LeftShift;
    private const int _selectKey = 0;
    private const int _actionKey = 1;

    private Camera CameraMain
    {
        get
        {
            if (_main == null)
                _main = Camera.main;
            return _main;
        }
    }

    private Ray CameraRay => CameraMain.ScreenPointToRay(Input.mousePosition);

    private bool PerformCast(out RaycastHit info)
    {
        return Physics.Raycast(CameraRay, out info);
    }

    private void Update()
    {
        if (_commandable != null && Input.GetMouseButton(_actionKey))
        {
            RaycastHit info;
            if (PerformCast(out info))
            {
                var point = info.point;
                var unit = info.collider.GetComponentInParent<Actor>();


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
                var commandable = info.collider.GetComponentInParent<CommandableComponent>();
                if (commandable != null)
                    _commandable = commandable;
            }
        }
    }

    private FollowCommand GenerateFollow(Transform target)
    {
        return new FollowCommand(_commandable.gameObject, target);
    }

    private MoveToCommand GenerateMove(Vector3 target)
    {
        return new MoveToCommand(_commandable.gameObject, target);
    }

    private void AddOrQueueCommand(ICommand command)
    {
        if (Input.GetKey(_queueKey))
            _commandable.AddCommand(command);
        else
            _commandable.SetCommand(command);
    }
}