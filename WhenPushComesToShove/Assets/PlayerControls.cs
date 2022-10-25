//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.3.0
//     from Assets/PlayerControls.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerControls : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""PlayerMovement"",
            ""id"": ""962a1bdc-9b74-45d9-a61c-f377a1348b56"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""PassThrough"",
                    ""id"": ""d41afcae-c781-49f7-8164-6d78af2538cd"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Select"",
                    ""type"": ""Button"",
                    ""id"": ""aae4f307-1222-46e4-bc30-1fecfcf047b2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""LightShove"",
                    ""type"": ""Button"",
                    ""id"": ""9568548c-dd93-4929-a840-31485dfc3dd7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press"",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""HeavyShove"",
                    ""type"": ""Button"",
                    ""id"": ""3946c64e-94c7-4a92-a918-61c8cd8bc051"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(pressPoint=0.01)"",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Dash"",
                    ""type"": ""Button"",
                    ""id"": ""9288ac1c-d24f-4b10-a0d2-5db012570a45"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Aim"",
                    ""type"": ""PassThrough"",
                    ""id"": ""fccec25c-e7df-4fa6-b42f-4f3fa1931342"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""69ccd0ec-5926-4b4e-82f5-417f670d649d"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": ""StickDeadzone"",
                    ""groups"": ""Xbox Controller Scheme"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""0f8c85fd-0d05-46b9-a5af-8ce2151b4e34"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""bae6ce3f-9376-4e5b-bb88-ce4fc67c8790"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Xbox Controller Scheme"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""a10909ee-b659-4ed9-ab7c-b6019d88a348"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Xbox Controller Scheme"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""6eed7d99-513d-4100-8eed-0488792bcc79"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Xbox Controller Scheme"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""c2f7c745-21f2-4836-a15e-cbf084a94912"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Xbox Controller Scheme"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""2b895834-6540-4ef0-9f42-b5e59842c99b"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""085a9295-da1c-401a-b9cb-5627f2b112b3"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LightShove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8ed3f68e-4ca7-4d45-a9fa-57caa2b7ed3c"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Xbox Controller Scheme"",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6ba43245-6dce-42e9-bab1-1eaa619841f1"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": ""StickDeadzone"",
                    ""groups"": ""Xbox Controller Scheme"",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a4028490-84d2-4288-bd39-f48f08757c39"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HeavyShove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Xbox Controller Scheme"",
            ""bindingGroup"": ""Xbox Controller Scheme"",
            ""devices"": [
                {
                    ""devicePath"": ""<XInputController>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // PlayerMovement
        m_PlayerMovement = asset.FindActionMap("PlayerMovement", throwIfNotFound: true);
        m_PlayerMovement_Movement = m_PlayerMovement.FindAction("Movement", throwIfNotFound: true);
        m_PlayerMovement_Select = m_PlayerMovement.FindAction("Select", throwIfNotFound: true);
        m_PlayerMovement_LightShove = m_PlayerMovement.FindAction("LightShove", throwIfNotFound: true);
        m_PlayerMovement_HeavyShove = m_PlayerMovement.FindAction("HeavyShove", throwIfNotFound: true);
        m_PlayerMovement_Dash = m_PlayerMovement.FindAction("Dash", throwIfNotFound: true);
        m_PlayerMovement_Aim = m_PlayerMovement.FindAction("Aim", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
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
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // PlayerMovement
    private readonly InputActionMap m_PlayerMovement;
    private IPlayerMovementActions m_PlayerMovementActionsCallbackInterface;
    private readonly InputAction m_PlayerMovement_Movement;
    private readonly InputAction m_PlayerMovement_Select;
    private readonly InputAction m_PlayerMovement_LightShove;
    private readonly InputAction m_PlayerMovement_HeavyShove;
    private readonly InputAction m_PlayerMovement_Dash;
    private readonly InputAction m_PlayerMovement_Aim;
    public struct PlayerMovementActions
    {
        private @PlayerControls m_Wrapper;
        public PlayerMovementActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_PlayerMovement_Movement;
        public InputAction @Select => m_Wrapper.m_PlayerMovement_Select;
        public InputAction @LightShove => m_Wrapper.m_PlayerMovement_LightShove;
        public InputAction @HeavyShove => m_Wrapper.m_PlayerMovement_HeavyShove;
        public InputAction @Dash => m_Wrapper.m_PlayerMovement_Dash;
        public InputAction @Aim => m_Wrapper.m_PlayerMovement_Aim;
        public InputActionMap Get() { return m_Wrapper.m_PlayerMovement; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerMovementActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerMovementActions instance)
        {
            if (m_Wrapper.m_PlayerMovementActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnMovement;
                @Select.started -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnSelect;
                @Select.performed -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnSelect;
                @Select.canceled -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnSelect;
                @LightShove.started -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnLightShove;
                @LightShove.performed -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnLightShove;
                @LightShove.canceled -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnLightShove;
                @HeavyShove.started -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnHeavyShove;
                @HeavyShove.performed -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnHeavyShove;
                @HeavyShove.canceled -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnHeavyShove;
                @Dash.started -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnDash;
                @Dash.performed -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnDash;
                @Dash.canceled -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnDash;
                @Aim.started -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnAim;
                @Aim.performed -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnAim;
                @Aim.canceled -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnAim;
            }
            m_Wrapper.m_PlayerMovementActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Select.started += instance.OnSelect;
                @Select.performed += instance.OnSelect;
                @Select.canceled += instance.OnSelect;
                @LightShove.started += instance.OnLightShove;
                @LightShove.performed += instance.OnLightShove;
                @LightShove.canceled += instance.OnLightShove;
                @HeavyShove.started += instance.OnHeavyShove;
                @HeavyShove.performed += instance.OnHeavyShove;
                @HeavyShove.canceled += instance.OnHeavyShove;
                @Dash.started += instance.OnDash;
                @Dash.performed += instance.OnDash;
                @Dash.canceled += instance.OnDash;
                @Aim.started += instance.OnAim;
                @Aim.performed += instance.OnAim;
                @Aim.canceled += instance.OnAim;
            }
        }
    }
    public PlayerMovementActions @PlayerMovement => new PlayerMovementActions(this);
    private int m_XboxControllerSchemeSchemeIndex = -1;
    public InputControlScheme XboxControllerSchemeScheme
    {
        get
        {
            if (m_XboxControllerSchemeSchemeIndex == -1) m_XboxControllerSchemeSchemeIndex = asset.FindControlSchemeIndex("Xbox Controller Scheme");
            return asset.controlSchemes[m_XboxControllerSchemeSchemeIndex];
        }
    }
    public interface IPlayerMovementActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnSelect(InputAction.CallbackContext context);
        void OnLightShove(InputAction.CallbackContext context);
        void OnHeavyShove(InputAction.CallbackContext context);
        void OnDash(InputAction.CallbackContext context);
        void OnAim(InputAction.CallbackContext context);
    }
}
