// GENERATED AUTOMATICALLY FROM 'Assets/Input/PlayerControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using Object = UnityEngine.Object;

public class PlayerControls : IInputActionCollection, IDisposable
{
    // Ability
    private readonly InputActionMap m_Ability;
    private readonly InputAction m_Ability_Alpha;
    private readonly InputAction m_Ability_Beta;
    private readonly InputAction m_Ability_Charlie;
    private readonly InputAction m_Ability_Delta;

    // Cursor
    private readonly InputActionMap m_Cursor;
    private readonly InputAction m_Cursor_Pos;

    // Movement
    private readonly InputActionMap m_Movement;
    private readonly InputAction m_Movement_Action;
    private readonly InputAction m_Movement_Follow;
    private readonly InputAction m_Movement_Move;
    private readonly InputAction m_Movement_Queue;
    private readonly InputAction m_Movement_Select;
    private IAbilityActions m_AbilityActionsCallbackInterface;
    private ICursorActions m_CursorActionsCallbackInterface;
    private IMovementActions m_MovementActionsCallbackInterface;

    public PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""Ability"",
            ""id"": ""73cd7b23-2c5a-46c4-b404-cc199447cce2"",
            ""actions"": [
                {
                    ""name"": ""Alpha"",
                    ""type"": ""Button"",
                    ""id"": ""a53e1257-ebb8-46d0-b7c5-daf97651044b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Beta"",
                    ""type"": ""Button"",
                    ""id"": ""dbc63152-fc21-46a1-bc6d-0e24ac2d913c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Charlie"",
                    ""type"": ""Button"",
                    ""id"": ""c10dd44c-2d59-4777-9a62-2d8ecf6b355c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Delta"",
                    ""type"": ""Button"",
                    ""id"": ""4129705e-580e-44b4-977f-48cf0412d03e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""09f5cfe2-d983-472a-adc8-f1efc151107c"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Alpha"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""037ec305-300b-4a8c-b32b-6e835ea1fdca"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Beta"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0f015767-a0d9-4509-bd80-d250b9004d80"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Charlie"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2a918cec-5315-451f-9a82-3f182515a45e"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Delta"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Movement"",
            ""id"": ""b5cc1b18-59f7-4419-8509-89a8c7ead7ab"",
            ""actions"": [
                {
                    ""name"": ""Follow"",
                    ""type"": ""Button"",
                    ""id"": ""1a6368b6-cb3e-413d-a2b6-aa78891fd190"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Move"",
                    ""type"": ""Button"",
                    ""id"": ""14b3e96a-14cc-49c0-9523-5ce034d71180"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Queue"",
                    ""type"": ""Button"",
                    ""id"": ""edd74ac0-5ac4-4a90-ac66-e26d4cdaabdb"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Select"",
                    ""type"": ""Button"",
                    ""id"": ""19f585fa-f5bb-45a5-851e-73500461912a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Action"",
                    ""type"": ""Button"",
                    ""id"": ""5e39d64a-d760-4638-8e98-16abafa113e6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""a91fac8f-dd07-47a7-af52-054120fbaf43"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Follow"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7b91d6ad-beda-40a1-aec0-e6254d8e8697"",
                    ""path"": ""<Keyboard>/m"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d2ad3f57-97d0-4af8-83eb-b91559d27c60"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Queue"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""674c4e94-dbee-4de0-96cb-650cb1f94fd5"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ff28c079-b4ad-4f2b-a5d0-191ad96d22c1"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Action"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Cursor"",
            ""id"": ""7067948e-0a70-4a69-9d32-26f310251c59"",
            ""actions"": [
                {
                    ""name"": ""Pos"",
                    ""type"": ""Value"",
                    ""id"": ""1226c9e9-d7a7-4fe0-9ed1-c5a60d9b483f"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""a0fd2a62-ade0-4f5f-8e64-49f6c26589c9"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pos"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Ability
        m_Ability = asset.FindActionMap("Ability", true);
        m_Ability_Alpha = m_Ability.FindAction("Alpha", true);
        m_Ability_Beta = m_Ability.FindAction("Beta", true);
        m_Ability_Charlie = m_Ability.FindAction("Charlie", true);
        m_Ability_Delta = m_Ability.FindAction("Delta", true);
        // Movement
        m_Movement = asset.FindActionMap("Movement", true);
        m_Movement_Follow = m_Movement.FindAction("Follow", true);
        m_Movement_Move = m_Movement.FindAction("Move", true);
        m_Movement_Queue = m_Movement.FindAction("Queue", true);
        m_Movement_Select = m_Movement.FindAction("Select", true);
        m_Movement_Action = m_Movement.FindAction("Action", true);
        // Cursor
        m_Cursor = asset.FindActionMap("Cursor", true);
        m_Cursor_Pos = m_Cursor.FindAction("Pos", true);
    }

    public InputActionAsset asset { get; }
    public AbilityActions Ability => new AbilityActions(this);
    public MovementActions Movement => new MovementActions(this);
    public CursorActions Cursor => new CursorActions(this);

    public void Dispose()
    {
        Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public struct AbilityActions
    {
        private readonly PlayerControls m_Wrapper;

        public AbilityActions(PlayerControls wrapper)
        {
            m_Wrapper = wrapper;
        }

        public InputAction Alpha => m_Wrapper.m_Ability_Alpha;
        public InputAction Beta => m_Wrapper.m_Ability_Beta;
        public InputAction Charlie => m_Wrapper.m_Ability_Charlie;
        public InputAction Delta => m_Wrapper.m_Ability_Delta;

        public InputActionMap Get()
        {
            return m_Wrapper.m_Ability;
        }

        public void Enable()
        {
            Get().Enable();
        }

        public void Disable()
        {
            Get().Disable();
        }

        public bool enabled => Get().enabled;

        public static implicit operator InputActionMap(AbilityActions set)
        {
            return set.Get();
        }

        public void SetCallbacks(IAbilityActions instance)
        {
            if (m_Wrapper.m_AbilityActionsCallbackInterface != null)
            {
                Alpha.started -= m_Wrapper.m_AbilityActionsCallbackInterface.OnAlpha;
                Alpha.performed -= m_Wrapper.m_AbilityActionsCallbackInterface.OnAlpha;
                Alpha.canceled -= m_Wrapper.m_AbilityActionsCallbackInterface.OnAlpha;
                Beta.started -= m_Wrapper.m_AbilityActionsCallbackInterface.OnBeta;
                Beta.performed -= m_Wrapper.m_AbilityActionsCallbackInterface.OnBeta;
                Beta.canceled -= m_Wrapper.m_AbilityActionsCallbackInterface.OnBeta;
                Charlie.started -= m_Wrapper.m_AbilityActionsCallbackInterface.OnCharlie;
                Charlie.performed -= m_Wrapper.m_AbilityActionsCallbackInterface.OnCharlie;
                Charlie.canceled -= m_Wrapper.m_AbilityActionsCallbackInterface.OnCharlie;
                Delta.started -= m_Wrapper.m_AbilityActionsCallbackInterface.OnDelta;
                Delta.performed -= m_Wrapper.m_AbilityActionsCallbackInterface.OnDelta;
                Delta.canceled -= m_Wrapper.m_AbilityActionsCallbackInterface.OnDelta;
            }

            m_Wrapper.m_AbilityActionsCallbackInterface = instance;
            if (instance != null)
            {
                Alpha.started += instance.OnAlpha;
                Alpha.performed += instance.OnAlpha;
                Alpha.canceled += instance.OnAlpha;
                Beta.started += instance.OnBeta;
                Beta.performed += instance.OnBeta;
                Beta.canceled += instance.OnBeta;
                Charlie.started += instance.OnCharlie;
                Charlie.performed += instance.OnCharlie;
                Charlie.canceled += instance.OnCharlie;
                Delta.started += instance.OnDelta;
                Delta.performed += instance.OnDelta;
                Delta.canceled += instance.OnDelta;
            }
        }
    }

    public struct MovementActions
    {
        private readonly PlayerControls m_Wrapper;

        public MovementActions(PlayerControls wrapper)
        {
            m_Wrapper = wrapper;
        }

        public InputAction Follow => m_Wrapper.m_Movement_Follow;
        public InputAction Move => m_Wrapper.m_Movement_Move;
        public InputAction Queue => m_Wrapper.m_Movement_Queue;
        public InputAction Select => m_Wrapper.m_Movement_Select;
        public InputAction Action => m_Wrapper.m_Movement_Action;

        public InputActionMap Get()
        {
            return m_Wrapper.m_Movement;
        }

        public void Enable()
        {
            Get().Enable();
        }

        public void Disable()
        {
            Get().Disable();
        }

        public bool enabled => Get().enabled;

        public static implicit operator InputActionMap(MovementActions set)
        {
            return set.Get();
        }

        public void SetCallbacks(IMovementActions instance)
        {
            if (m_Wrapper.m_MovementActionsCallbackInterface != null)
            {
                Follow.started -= m_Wrapper.m_MovementActionsCallbackInterface.OnFollow;
                Follow.performed -= m_Wrapper.m_MovementActionsCallbackInterface.OnFollow;
                Follow.canceled -= m_Wrapper.m_MovementActionsCallbackInterface.OnFollow;
                Move.started -= m_Wrapper.m_MovementActionsCallbackInterface.OnMove;
                Move.performed -= m_Wrapper.m_MovementActionsCallbackInterface.OnMove;
                Move.canceled -= m_Wrapper.m_MovementActionsCallbackInterface.OnMove;
                Queue.started -= m_Wrapper.m_MovementActionsCallbackInterface.OnQueue;
                Queue.performed -= m_Wrapper.m_MovementActionsCallbackInterface.OnQueue;
                Queue.canceled -= m_Wrapper.m_MovementActionsCallbackInterface.OnQueue;
                Select.started -= m_Wrapper.m_MovementActionsCallbackInterface.OnSelect;
                Select.performed -= m_Wrapper.m_MovementActionsCallbackInterface.OnSelect;
                Select.canceled -= m_Wrapper.m_MovementActionsCallbackInterface.OnSelect;
                Action.started -= m_Wrapper.m_MovementActionsCallbackInterface.OnAction;
                Action.performed -= m_Wrapper.m_MovementActionsCallbackInterface.OnAction;
                Action.canceled -= m_Wrapper.m_MovementActionsCallbackInterface.OnAction;
            }

            m_Wrapper.m_MovementActionsCallbackInterface = instance;
            if (instance != null)
            {
                Follow.started += instance.OnFollow;
                Follow.performed += instance.OnFollow;
                Follow.canceled += instance.OnFollow;
                Move.started += instance.OnMove;
                Move.performed += instance.OnMove;
                Move.canceled += instance.OnMove;
                Queue.started += instance.OnQueue;
                Queue.performed += instance.OnQueue;
                Queue.canceled += instance.OnQueue;
                Select.started += instance.OnSelect;
                Select.performed += instance.OnSelect;
                Select.canceled += instance.OnSelect;
                Action.started += instance.OnAction;
                Action.performed += instance.OnAction;
                Action.canceled += instance.OnAction;
            }
        }
    }

    public struct CursorActions
    {
        private readonly PlayerControls m_Wrapper;

        public CursorActions(PlayerControls wrapper)
        {
            m_Wrapper = wrapper;
        }

        public InputAction Pos => m_Wrapper.m_Cursor_Pos;

        public InputActionMap Get()
        {
            return m_Wrapper.m_Cursor;
        }

        public void Enable()
        {
            Get().Enable();
        }

        public void Disable()
        {
            Get().Disable();
        }

        public bool enabled => Get().enabled;

        public static implicit operator InputActionMap(CursorActions set)
        {
            return set.Get();
        }

        public void SetCallbacks(ICursorActions instance)
        {
            if (m_Wrapper.m_CursorActionsCallbackInterface != null)
            {
                Pos.started -= m_Wrapper.m_CursorActionsCallbackInterface.OnPos;
                Pos.performed -= m_Wrapper.m_CursorActionsCallbackInterface.OnPos;
                Pos.canceled -= m_Wrapper.m_CursorActionsCallbackInterface.OnPos;
            }

            m_Wrapper.m_CursorActionsCallbackInterface = instance;
            if (instance != null)
            {
                Pos.started += instance.OnPos;
                Pos.performed += instance.OnPos;
                Pos.canceled += instance.OnPos;
            }
        }
    }

    public interface IAbilityActions
    {
        void OnAlpha(InputAction.CallbackContext context);
        void OnBeta(InputAction.CallbackContext context);
        void OnCharlie(InputAction.CallbackContext context);
        void OnDelta(InputAction.CallbackContext context);
    }

    public interface IMovementActions
    {
        void OnFollow(InputAction.CallbackContext context);
        void OnMove(InputAction.CallbackContext context);
        void OnQueue(InputAction.CallbackContext context);
        void OnSelect(InputAction.CallbackContext context);
        void OnAction(InputAction.CallbackContext context);
    }

    public interface ICursorActions
    {
        void OnPos(InputAction.CallbackContext context);
    }
}