//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/MRTemplateAssets/MRTemplateInputActions.inputactions
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

public partial class @MRTemplateInputActions: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @MRTemplateInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""MRTemplateInputActions"",
    ""maps"": [
        {
            ""name"": ""LeftHand"",
            ""id"": ""6b3ecedb-5203-4b11-9c4e-bc024d72a9d0"",
            ""actions"": [
                {
                    ""name"": ""Palm Position"",
                    ""type"": ""Value"",
                    ""id"": ""77df7cdf-9376-4a7e-bb26-b9997690ad71"",
                    ""expectedControlType"": ""Vector3"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Palm Rotation"",
                    ""type"": ""Value"",
                    ""id"": ""658d8330-31f2-494e-951f-5cfba90acb5e"",
                    ""expectedControlType"": ""Quaternion"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""72471180-bfbf-452d-8e11-a331889ea2c3"",
                    ""path"": ""<XRHandDevice>{LeftHand}/gripPosition"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Palm Position"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b4a7d129-bdfc-4497-ba40-1b2dc88a9811"",
                    ""path"": ""<XRHandDevice>{LeftHand}/gripRotation"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Palm Rotation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""RightHand"",
            ""id"": ""67551466-8b27-4293-861f-a0332e976068"",
            ""actions"": [
                {
                    ""name"": ""Palm Position"",
                    ""type"": ""Value"",
                    ""id"": ""cf7ecd0d-e5fe-4b4c-8e2b-7eba9fca00a7"",
                    ""expectedControlType"": ""Vector3"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Palm Rotation"",
                    ""type"": ""Value"",
                    ""id"": ""fd34d4f8-6f08-474b-9972-449bfc5eb815"",
                    ""expectedControlType"": ""Quaternion"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""13b87c9e-834d-4494-a900-438e9c45c3f1"",
                    ""path"": ""<XRHandDevice>{RightHand}/gripPosition"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Palm Position"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9ce6209b-d2ed-4182-906c-d1d78e4b0d58"",
                    ""path"": ""<XRHandDevice>{RightHand}/gripRotation"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Palm Rotation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // LeftHand
        m_LeftHand = asset.FindActionMap("LeftHand", throwIfNotFound: true);
        m_LeftHand_PalmPosition = m_LeftHand.FindAction("Palm Position", throwIfNotFound: true);
        m_LeftHand_PalmRotation = m_LeftHand.FindAction("Palm Rotation", throwIfNotFound: true);
        // RightHand
        m_RightHand = asset.FindActionMap("RightHand", throwIfNotFound: true);
        m_RightHand_PalmPosition = m_RightHand.FindAction("Palm Position", throwIfNotFound: true);
        m_RightHand_PalmRotation = m_RightHand.FindAction("Palm Rotation", throwIfNotFound: true);
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

    // LeftHand
    private readonly InputActionMap m_LeftHand;
    private List<ILeftHandActions> m_LeftHandActionsCallbackInterfaces = new List<ILeftHandActions>();
    private readonly InputAction m_LeftHand_PalmPosition;
    private readonly InputAction m_LeftHand_PalmRotation;
    public struct LeftHandActions
    {
        private @MRTemplateInputActions m_Wrapper;
        public LeftHandActions(@MRTemplateInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @PalmPosition => m_Wrapper.m_LeftHand_PalmPosition;
        public InputAction @PalmRotation => m_Wrapper.m_LeftHand_PalmRotation;
        public InputActionMap Get() { return m_Wrapper.m_LeftHand; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(LeftHandActions set) { return set.Get(); }
        public void AddCallbacks(ILeftHandActions instance)
        {
            if (instance == null || m_Wrapper.m_LeftHandActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_LeftHandActionsCallbackInterfaces.Add(instance);
            @PalmPosition.started += instance.OnPalmPosition;
            @PalmPosition.performed += instance.OnPalmPosition;
            @PalmPosition.canceled += instance.OnPalmPosition;
            @PalmRotation.started += instance.OnPalmRotation;
            @PalmRotation.performed += instance.OnPalmRotation;
            @PalmRotation.canceled += instance.OnPalmRotation;
        }

        private void UnregisterCallbacks(ILeftHandActions instance)
        {
            @PalmPosition.started -= instance.OnPalmPosition;
            @PalmPosition.performed -= instance.OnPalmPosition;
            @PalmPosition.canceled -= instance.OnPalmPosition;
            @PalmRotation.started -= instance.OnPalmRotation;
            @PalmRotation.performed -= instance.OnPalmRotation;
            @PalmRotation.canceled -= instance.OnPalmRotation;
        }

        public void RemoveCallbacks(ILeftHandActions instance)
        {
            if (m_Wrapper.m_LeftHandActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(ILeftHandActions instance)
        {
            foreach (var item in m_Wrapper.m_LeftHandActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_LeftHandActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public LeftHandActions @LeftHand => new LeftHandActions(this);

    // RightHand
    private readonly InputActionMap m_RightHand;
    private List<IRightHandActions> m_RightHandActionsCallbackInterfaces = new List<IRightHandActions>();
    private readonly InputAction m_RightHand_PalmPosition;
    private readonly InputAction m_RightHand_PalmRotation;
    public struct RightHandActions
    {
        private @MRTemplateInputActions m_Wrapper;
        public RightHandActions(@MRTemplateInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @PalmPosition => m_Wrapper.m_RightHand_PalmPosition;
        public InputAction @PalmRotation => m_Wrapper.m_RightHand_PalmRotation;
        public InputActionMap Get() { return m_Wrapper.m_RightHand; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(RightHandActions set) { return set.Get(); }
        public void AddCallbacks(IRightHandActions instance)
        {
            if (instance == null || m_Wrapper.m_RightHandActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_RightHandActionsCallbackInterfaces.Add(instance);
            @PalmPosition.started += instance.OnPalmPosition;
            @PalmPosition.performed += instance.OnPalmPosition;
            @PalmPosition.canceled += instance.OnPalmPosition;
            @PalmRotation.started += instance.OnPalmRotation;
            @PalmRotation.performed += instance.OnPalmRotation;
            @PalmRotation.canceled += instance.OnPalmRotation;
        }

        private void UnregisterCallbacks(IRightHandActions instance)
        {
            @PalmPosition.started -= instance.OnPalmPosition;
            @PalmPosition.performed -= instance.OnPalmPosition;
            @PalmPosition.canceled -= instance.OnPalmPosition;
            @PalmRotation.started -= instance.OnPalmRotation;
            @PalmRotation.performed -= instance.OnPalmRotation;
            @PalmRotation.canceled -= instance.OnPalmRotation;
        }

        public void RemoveCallbacks(IRightHandActions instance)
        {
            if (m_Wrapper.m_RightHandActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IRightHandActions instance)
        {
            foreach (var item in m_Wrapper.m_RightHandActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_RightHandActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public RightHandActions @RightHand => new RightHandActions(this);
    public interface ILeftHandActions
    {
        void OnPalmPosition(InputAction.CallbackContext context);
        void OnPalmRotation(InputAction.CallbackContext context);
    }
    public interface IRightHandActions
    {
        void OnPalmPosition(InputAction.CallbackContext context);
        void OnPalmRotation(InputAction.CallbackContext context);
    }
}