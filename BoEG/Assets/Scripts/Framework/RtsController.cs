﻿using System;
using MobaGame.Framework.Core;
using MobaGame.Framework.Core.Modules;
using MobaGame.Framework.Core.Modules.Commands;
using MobaGame.Framework.Types;
using MobaGame.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MobaGame.Framework
{
    public static class InputActionHelper
    {
        //
        public static bool ButtonPressed(this InputAction action) => action.ReadValue<float>() >= 0.5f;

    }
    public class RtsController : MonoBehaviour
    {
#pragma warning disable 0649

        private IAbilitiable _abilitiableHACK;
        [SerializeField] private Actor _actor;
        [SerializeField] private ActorPanel _panel;
        [SerializeField] private CustomCursor _cursor;
        private ICommandable _commandable;

        private PlayerControls _controls;

        private Camera _main;
        private Ray _ray;
#pragma warning restore 0649


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
                if (_panel != null)
                    _panel.SetTarget(null);
                return;
            }

            _commandable = _actor.GetModule<ICommandable>();
            _abilitiableHACK = _actor.GetModule<IAbilitiable>();
            if (_panel != null)
                _panel.SetTarget(_actor);
        }

        private Action<InputAction.CallbackContext> AbilityOnStarted(int index)
        {
            void InternalOnStarted(InputAction.CallbackContext context)
            {
                if (_abilitiableHACK != null && _abilitiableHACK.Abilities.Count > index)
                {
                    var ability = _abilitiableHACK.Abilities[index];
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


                    if (unit != null && _controls.Movement.Move.ButtonPressed()) // Follow
                        AddOrQueueCommand(GenerateFollow(unit.transform));
                    else if (_controls.Movement.Follow.ButtonPressed()) // Move
                        AddOrQueueCommand(GenerateMove(point));
                    else if (_controls.Movement.Attack.ButtonPressed()) //Attack pressed, do attack move
                        // if(unit != null)
                        //     AddOrQueueCommand(GenerateAttackMove());
                        // else
                        AddOrQueueCommand(GenerateAttackMove(point));
                    else if (unit != null) // Follow unit
                        AddOrQueueCommand(GenerateFollow(unit.transform));
                    else //Move to target
                        AddOrQueueCommand(GenerateMove(point));
                }
            }
        }
		private void LateUpdate()
		{
            if (_cursor == null)
                return;

            if (_controls.Movement.Attack.ButtonPressed())
                _cursor.Mode = CustomCursor.CursorState.Attacking;
            else
                _cursor.Mode = CustomCursor.CursorState.Default;
		}

		private ICommand GenerateAttackMove(Vector3 target)
        {
            return new AttackMoveCommand(_actor.gameObject, target);
        }

        private bool PerformCast(out RaycastHit info)
        {
            var ray = CameraRay;

            // Debug.Log("RTS:\t" + ray);
            return Physics.Raycast(ray, out info, 100f, (int) (LayerMaskFlag.Entity | LayerMaskFlag.World));
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
}