using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ControllerPositionGUI : MonoBehaviour {

    [SerializeField] private Transform LeftController;
    [SerializeField] private Transform RightController;
    [SerializeField] private TextMeshProUGUI LeftControllerText;
    [SerializeField] private TextMeshProUGUI RightControllerText;

    private Vector3 leftPosition = Vector3.zero;
    private Vector3 rightPosition = Vector3.zero;
    private Vector3 leftRotation = Vector3.zero;
    private Vector3 rightRotation = Vector3.zero;
    private UnityEngine.XR.InputDevice leftController;
    private UnityEngine.XR.InputDevice rightController;

    public void Start() {
        var leftHandDevices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevicesAtXRNode(UnityEngine.XR.XRNode.LeftHand, leftHandDevices);
        var rightHandDevices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevicesAtXRNode(UnityEngine.XR.XRNode.RightHand, rightHandDevices);

        if (leftHandDevices.Count == 1) {
            UnityEngine.XR.InputDevice device = leftHandDevices[0];
            LeftControllerText.text = string.Format("Device name '{0}' with role '{1}'", device.name, device.characteristics.ToString());
        } else if (leftHandDevices.Count > 1) {
            LeftControllerText.text = "Found more than one left hand!";
        }

        if (rightHandDevices.Count == 1) {
            UnityEngine.XR.InputDevice device = rightHandDevices[0];
            RightControllerText.text = string.Format("Device name '{0}' with role '{1}'", device.name, device.characteristics.ToString());
        } else if (rightHandDevices.Count > 1) {
            RightControllerText.text = "Found more than one right hand!";
        }
    }

    public void Update() {
        leftPosition = LeftController.position;
        leftRotation = LeftController.eulerAngles;
        rightPosition = RightController.position;
        rightRotation = RightController.eulerAngles;
                
        bool leftTriggerValue;
        if (leftController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out leftTriggerValue) && leftTriggerValue) {
            LeftControllerText.text = "Trigger button is pressed.";
        } else {
            LeftControllerText.text = "Position (x,y,z):\n " + leftPosition.ToString() + "\n Rotation (p,y,r):\n " + leftRotation.ToString();
        }

        bool rightTriggerValue;
        if (rightController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out rightTriggerValue) && rightTriggerValue) {
            RightControllerText.text = "Trigger button is pressed.";
        } else {
            RightControllerText.text = "Position (x,y,z):\n" + rightPosition.ToString() + "\n Rotation:\n" + rightRotation.ToString();
        }

    }

    
}

