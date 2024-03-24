using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ControllerPositionGUI : MonoBehaviour {

    [SerializeField] private Transform LeftController;
    [SerializeField] private Transform RightController;
    [SerializeField] private TextMeshProUGUI LeftControllerText;
    [SerializeField] private TextMeshProUGUI RightControllerText;
    [SerializeField] private Image leftBarImage;
    [SerializeField] private Image rightBarImage;
    

    private Vector3 leftPosition = Vector3.zero;
    private Vector3 rightPosition = Vector3.zero;
    private Vector3 leftRotation = Vector3.zero;
    private Vector3 rightRotation = Vector3.zero;
    private float leftMinY = 0f;
    private float rightMinY = 0f;
    private float leftMaxY = 2.5f;
    private float rightMaxY = 2.5f;
    private ArrowKeys playerInputActions;

    public void Awake() {
        playerInputActions = new ArrowKeys();
        playerInputActions.Keyboard.Enable();
        playerInputActions.Keyboard.LongArrows.performed += Long_Arrows_performed;
    }


    public void Update() {
        leftPosition = LeftController.position;
        leftRotation = LeftController.eulerAngles;
        rightPosition = RightController.position;
        rightRotation = RightController.eulerAngles;
                
        
        LeftControllerText.text = "Position (x,y,z):\n " + leftPosition.ToString() + "\n Rotation (p,y,r):\n " + leftRotation.ToString();
        RightControllerText.text = "Position (x,y,z):\n" + rightPosition.ToString() + "\n Rotation (p,y,r):\n" + rightRotation.ToString();

        leftBarImage.fillAmount = Normalize(leftPosition.y, leftMinY, leftMaxY);
        rightBarImage.fillAmount = Normalize(rightPosition.y, rightMinY, rightMaxY);
    }

    /// <summary>
    /// Method <c>Long_Arrows_performed</c> is an event handler for long presses of the arrow keys.
    /// </summary>
    /// <param name="context">Points back to the action input event to be handled.</param>
    private void Long_Arrows_performed(InputAction.CallbackContext context) {
        if (context.performed) {
            InputControl keyPressed = context.control;
            if (keyPressed.name == "upArrow") {
                Debug.Log("Up Arrow long-pressed!");
                leftMaxY = LeftController.position.y;
            }
            if (keyPressed.name == "downArrow") {
                Debug.Log("Down Arrow long-pressed!");
                leftMinY = LeftController.position.y;
            }
            if (keyPressed.name == "leftArrow") {
                Debug.Log("Left Arrow long-pressed!");
                rightMinY = RightController.position.y;
            }
            if (keyPressed.name == "rightArrow") {
                Debug.Log("Right Arrow long-pressed!");
                rightMaxY = RightController.position.y;
            }
        }
    }

    /// <summary>
    /// Method <c>Normalize</c> rescales the input value to it's percentage of the value between 
    /// min and the max.
    /// </summary>
    /// <param name="value"></param> parameter to be rescaled
    /// <param name="min"></param> returns 0f if value <= min
    /// <param name="max"></param> returns 1f if value >= max
    /// <returns></returns>
    private float Normalize(float value, float min, float max) {
        float result = (value - min) / (max - min);
        if (result < 0f) { result = 0f; }
        if (result > 1f) {  result = 1f; }
        return result;
    }

}

