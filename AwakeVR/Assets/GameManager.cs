using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public GameObject[] shapes;
    public GameObject[] bins;

    private int currentShapeIndex = 0;
    private int currentBinIndex = 0;

    private enum SelectionMode { ShapeSelection, BinSelection };
    private SelectionMode currentMode = SelectionMode.ShapeSelection;

    private Keyboard keyboard;

    // Start is called before the first frame update
    void Start()
    {
        UpdateShapeHighlight();
        UpdateBinHighlight();
        keyboard = Keyboard.current;
    }

    // Update is called once per frame
    void Update()
    {           
        // Handle left / right arrow inputs
        if (keyboard.leftArrowKey.wasPressedThisFrame)
        {
            if (currentMode == SelectionMode.ShapeSelection)
            {
                currentShapeIndex = (currentShapeIndex - 1 + shapes.Length) % shapes.Length;
                UpdateShapeHighlight();
            }
            else if (currentMode == SelectionMode.BinSelection)
            {
                currentBinIndex = (currentBinIndex - 1 + bins.Length) % bins.Length;
                UpdateBinHighlight();
            }
        }
        else if (keyboard.rightArrowKey.wasPressedThisFrame)
        {
            if (currentMode == SelectionMode.ShapeSelection)
            {
                currentShapeIndex = (currentShapeIndex + 1) % shapes.Length;
                UpdateShapeHighlight();
            }
            else if (currentMode == SelectionMode.BinSelection)
            {
                currentBinIndex = (currentBinIndex + 1) % bins.Length;
                UpdateBinHighlight();
            }
        }

        // Handle space bar for selection
        if (keyboard.spaceKey.wasPressedThisFrame)
        {
            if (currentMode == SelectionMode.ShapeSelection)
            {
                Debug.Log("Shape Selected: " + shapes[currentShapeIndex].name);
                currentMode = SelectionMode.BinSelection;
            }
            else if (currentMode == SelectionMode.BinSelection)
            {
                PlaceShapeInBin(); 
                currentMode = SelectionMode.ShapeSelection;

                // Reset bin selection index
                currentBinIndex = 0;
                UpdateBinHighlight();
            }
        }
    }

    void UpdateShapeHighlight()
    {
        // Loop through all shpaes and update their appearance
        for (int i = 0; i < shapes.Length; i++)
        {
            Renderer rend = shapes[i].GetComponent<Renderer>();
            if (rend != null)
            {
                if (i == currentShapeIndex)
                {
                    rend.material.color = Color.green; // Highlight the selected shape
                }
                else
                {
                    rend.material.color = Color.white; // Default color.
                }
            }
        }
    }

    void UpdateBinHighlight()
    {
        // Loop through all bins and update their appearance
        for (int i = 0; i < bins.Length; i++)
        {
            Renderer rend = bins[i].GetComponent<Renderer>();
            if (rend != null)
            {
                if (i == currentBinIndex)
                {
                    rend.material.color = Color.yellow; // Highlight the selected bin
                }
                else
                {
                    rend.material.color = Color.gray; // Default color.
                }
            }
        }
    }

    void PlaceShapeInBin()
    {
        // Get the selected shape and bin
        GameObject selectedShape = shapes[currentShapeIndex];
        GameObject selectedBin = bins[currentBinIndex];

        // Move the shape to the bin's position
        selectedShape.transform.position = selectedBin.transform.position;
        
        // Add in order logic here
        Debug.Log(selectedShape.name + " placed in " + selectedBin.name);
    }

    
}
