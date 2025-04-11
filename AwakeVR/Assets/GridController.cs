using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GridController : MonoBehaviour
{
    public GameObject[,] Grid {get; private set;}
    [Header("Positioning Options")]
    public bool useFixedPosition = true;
    public Vector3 fixedPosition = new Vector3(0, 1.5f, 2.5f);
    public bool maintainRotation = false;

    public GameObject cubePrefab;
    public float spacing = 1.2f;
    // public GameObject[,] grid = new GameObject[3,3];
    private Vector2Int currentPosition = new Vector2Int(0,0);
    private Color originalColor;
    private Keyboard keyboard;
    private RandomPatternGenerator randomPatternGenerator;
    // Make selectedCubes a list of x and y values representing the cube squares.
    public List<Vector2Int> selectedCubes = new List<Vector2Int>();

    void Awake()
    {
        if (useFixedPosition)
        {
            transform.position = fixedPosition;
        }
        
        Grid = new GameObject[3, 3];
        CreateGrid();

        
        if (!maintainRotation) transform.rotation = Quaternion.identity;
        
        randomPatternGenerator = GetComponent<RandomPatternGenerator>();
        originalColor = cubePrefab.GetComponent<Renderer>().sharedMaterial.color;
    }

    void Start()
    {   
        UpdateHighlight();
        keyboard = Keyboard.current;
    }

    void CreateGrid()
    {
        Grid = new GameObject[3, 3];

        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                float xPos = (x-1) * spacing;
                float yPos = (y-1) * spacing;

                Vector3 position = new Vector3(xPos, yPos, 0);
                GameObject cube = Instantiate(cubePrefab, transform);
                cube.transform.localPosition = position;
                cube.name = $"Cube_{x}_{y}";
                Grid[x, y] = cube;
                Grid[x, y].GetComponent<CubeController>().gridPosition = new Vector2Int(x, y);
            }
        }
    }

    void Update()
    {
        if (GameManager.Instance.CurrentState == GameManager.GameState.Selection)
        {
            HandleInput();
        }
    }

    void HandleInput()
    {
        Vector2Int newPosition = currentPosition;
        if (keyboard.nKey.wasPressedThisFrame)        {
            // call GenerateNewPattern from RandomPatternGenerator.cs
            GameManager.Instance.ResetSelections();
            selectedCubes.Clear();
            ResetAllColors();
            randomPatternGenerator.GenerateNewPattern();
        }
        if (keyboard.rKey.wasPressedThisFrame)        {
            // call RepeatPattern from GameManager.cs
            GameManager.Instance.RepeatPattern();
        }

        if (keyboard.leftArrowKey.wasPressedThisFrame) newPosition.x--;
        if (keyboard.rightArrowKey.wasPressedThisFrame) newPosition.x++;
        if (keyboard.upArrowKey.wasPressedThisFrame) newPosition.y++;
        if (keyboard.downArrowKey.wasPressedThisFrame) newPosition.y--;

        newPosition.x = Mathf.Clamp(newPosition.x, 0, 2);
        newPosition.y = Mathf.Clamp(newPosition.y, 0, 2);

        if (newPosition != currentPosition)
        {
            currentPosition = newPosition;
            UpdateHighlight();
        }
        bool pressed = false;
        if (keyboard.spaceKey.wasPressedThisFrame)
        {
            ToggleSelection();
        }
    }

    void UpdateHighlight()
    {
        foreach (GameObject cube in Grid)
        {
            CubeController controller = cube.GetComponent<CubeController>();
            controller.ResetColor();
        }

        GetCurrentCubeController().Highlight();
    }

    void ToggleSelection()
    {
        CubeController controller = GetCurrentCubeController();
        if (controller.isSelected)
        {
            selectedCubes.Remove(controller.gridPosition);
        }
        else
        {
            selectedCubes.Add(controller.gridPosition);
        }
        controller.ToggleSelection();

        if (selectedCubes.Count == randomPatternGenerator.patternLength)
        {
            if (CheckUserPattern())
            {
                Debug.Log("Pattern matched!");
                GameManager.Instance.ResetSelections();
                selectedCubes.Clear();
                ResetAllColors();
                GameManager.Instance.StartNewRound();
            }
            else
            {
                Debug.Log("Pattern mismatch.");
                GameManager.Instance.ResetSelections();
                selectedCubes.Clear();
                ResetAllColors();
                GameManager.Instance.RepeatPattern();
            }
        }
    }

    CubeController GetCurrentCubeController()
    {
        return Grid[currentPosition.x, currentPosition.y].GetComponent<CubeController>();
    }

    public void ResetAllColors()
    {
        foreach (GameObject cube in Grid)
        {
            CubeController controller = cube.GetComponent<CubeController>();
            controller.ResetToOriginal();
        }
    }

    public void SetInputEnabled(bool enabled)
    {
        this.enabled = enabled;
    }

    public void ResetToDefaultPosition()
    {
        currentPosition = new Vector2Int(1, 1);
        UpdateHighlight();
    }

    public bool CheckUserPattern()
    {
        if (randomPatternGenerator.patternSequence == null)
        {
            Debug.LogError("Pattern sequence is null.");
            return false;
        }
        if (selectedCubes.Count != randomPatternGenerator.patternLength)
        {
            Debug.LogError("Pattern length mismatch.");
            return false;
        }


        for (int i = 0; i < selectedCubes.Count; i++)
        {
            if (selectedCubes[i] != randomPatternGenerator.patternSequence[i])
            {
                Debug.Log($"User input at index {i} does not match pattern.");
                return false;
            }
        }

        return true;
    }

}
