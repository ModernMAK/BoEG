using System;
using Framework.Core;
using Framework.Core.Modules;
using Framework.Core.Modules.Commands;
using Triggers;
using UnityEngine;
using UnityEngine.InputSystem;

public class RtsController : MonoBehaviour
{
    private IAbilitiable _abilitiableHACK;
    [SerializeField] private Actor _actor;
    [SerializeField] private ActorPanel _panel;
    private ICommandable _commandable;

    private PlayerControls _controls;

    private Camera _main;
    private Ray _ray;


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
        get
        {
            var point = _controls.Cursor.Pos.ReadValue<Vector2>();
            // Debug.Log("CameraRay:\t"+point);
            var ray = CameraMain.ScreenPointToRay(point);
            return ray;
        }
    }

    private void Awake()
    {
        _controls = new PlayerControls();
        _controls.Movement.Action.started += ActionOnstarted;
        _controls.Movement.Select.started += SelectOnstarted;
        _controls.Ability.Alpha.started += AbilityOnStarted(0);
        _controls.Ability.Beta.started += AbilityOnStarted(1);
        _controls.Ability.Charlie.started += AbilityOnStarted(2);
        _controls.Ability.Delta.started += AbilityOnStarted(3);
        if (_actor != null)
            Select(_actor.gameObject);
    }

    private void OnEnable()
    {
        _controls.Enable();
    }

    private void OnDisable()
    {
        _controls.Disable();
    }

    private void Select(GameObject go)
    {
        _actor = go.GetComponent<Actor>();
        if (_actor == null)
        {
            _commandable = null;
            _abilitiableHACK = null;
            return;
        }

        _commandable = _actor.GetComponent<ICommandable>();
        _abilitiableHACK = _actor.GetComponent<IAbilitiable>();
        if (_panel != null)
            _panel.SetTarget(_actor.gameObject);
    }

    private Action<InputAction.CallbackContext> AbilityOnStarted(int index)
    {
        void InternalOnStarted(InputAction.CallbackContext context)
        {
            if (_abilitiableHACK != null && _abilitiableHACK.AbilityCount > index)
            {
                var ability = _abilitiableHACK.GetAbility(index);
                ability.SetupCast();
                ability.ConfirmCast();
            }
        }

        return InternalOnStarted;
    }

    private void SelectOnstarted(InputAction.CallbackContext obj)
    {
        RaycastHit info;
        if (PerformCast(out info)) Select(info.transform.gameObject);
    }

    private void ActionOnstarted(InputAction.CallbackContext obj)
    {
        if (_commandable != null)
        {
            RaycastHit info;
            if (PerformCast(out info))
            {
                var point = info.point;
                var unit = info.collider.GetComponentInParent<Actor>();


                if (unit != null && !_controls.Movement.Move.ReadValue<bool>())
                    AddOrQueueCommand(GenerateFollow(unit.transform));
                else if (!_controls.Movement.Follow.ReadValue<bool>())
                    AddOrQueueCommand(GenerateMove(point));
            }
        }
    }

    private bool PerformCast(out RaycastHit info)
    {
        var ray = CameraRay;

        // Debug.Log("RTS:\t" + ray);
        return Physics.Raycast(ray, out info, 100f, (int)(LayerMaskHelper.Entity | LayerMaskHelper.World));
    }


    private FollowCommand GenerateFollow(Transform target)
    {
        return new FollowCommand(_actor.gameObject, target);
    }

    private MoveToCommand GenerateMove(Vector3 target)
    {
        return new MoveToCommand(_actor.gameObject, target);
    }

    private void AddOrQueueCommand(ICommand command)
    {
        if (_controls.Movement.Queue.ReadValue<bool>())
            _commandable.AddCommand(command);
        else
            _commandable.SetCommand(command);
    }
}