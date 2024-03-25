using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ControllerPositionGUI : MonoBehaviour {

    [SerializeField] private Transform LeftController;
    [SerializeField] private Transform RightController;
    [SerializeField] private Image leftBarImage;
    [SerializeField] private Image rightBarImage;
    

    private Vector3 leftPosition = Vector3.zero;
    private Vector3 rightPosition = Vector3.zero;
    private float leftMinY = 0f;
    private float rightMinY = 0f;
    private float leftMaxY = 2.5f;
    private float rightMaxY = 2.5f;
    private ArrowKeys playerInputActions;

    
    public void Update() {
        leftPosition = LeftController.position;
        rightPosition = RightController.position;
        
        leftBarImage.fillAmount = Normalize(leftPosition.y, leftMinY, leftMaxY);
        rightBarImage.fillAmount = Normalize(rightPosition.y, rightMinY, rightMaxY);
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

    public void SetLeftMinY() { 
        leftMinY = LeftController.position.y;
    }

    public void SetLeftMaxY() { 
        leftMaxY = LeftController.position.y;
    }

    public void SetRightMinY() {  
        rightMinY = RightController.position.y;
    }

    public void SetRightMaxY() { 
        rightMaxY = RightController.position.y;
    }



}

